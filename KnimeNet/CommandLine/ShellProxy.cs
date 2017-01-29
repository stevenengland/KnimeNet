using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using KnimeNet.CommandLine.Types;
using KnimeNet.Environment.Types.Enums;
using KnimeNet.Extensions;
using OperatingSystem = KnimeNet.Environment.OperatingSystem;

namespace KnimeNet.CommandLine
{
    /// <summary>
    /// This instance manages the communication with KNIME.
    /// </summary>
    public class ShellProxy
    {
        /// <summary>
        /// The Path to the KNIME executable.
        /// </summary>
        private readonly string _knimeDir;

        /// <summary>
        /// The KNIME executable string.
        /// </summary>
        private readonly string _knimeApp;

        /// <summary>
        /// Used to cancel a pending execution.
        /// </summary>
        private CancellationTokenSource _executionCancellationTokenSource = default(CancellationTokenSource);

        /// <summary>
        /// Timeout in seconds before the proxy returns and tries to kill a running process. If left empty no timeout is set.
        /// </summary>
        public int? Timeout { get; set; }

        /// <summary>
        /// Whether or not a started process should be terminated after an error occured during batch execution.
        /// </summary>
        public bool KillOnError { get; set; }

        /// <summary>
        /// The command line argument package.
        /// </summary>
        public ArgumentBag Arguments {get; set; } = new ArgumentBag();

        /// <summary>
        /// Creates a new instance of <see cref="ShellProxy" />.
        /// </summary>
        /// <param name="knimeDir">The path to the KNIME executable. If left empty KNIME will be called without absolute path to the executable and must therefore be known in the PATH.</param>
        /// <exception cref="DirectoryNotFoundException">Invalid KNIME directory</exception>
        /// <exception cref="NotImplementedException">Could not determine operating system type</exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="FileNotFoundException">The KNIME executable could not be found</exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public ShellProxy(string knimeDir = null)
        {
            if (knimeDir != null)
            {
                if (!knimeDir.EndsWith(Path.DirectorySeparatorChar.ToString()))
                    knimeDir += Path.DirectorySeparatorChar;
                if (!Directory.Exists(knimeDir))
                    throw new DirectoryNotFoundException("Invalid KNIME directory");
            }

            _knimeDir = knimeDir;

            switch (OperatingSystem.GetOsType())
            {
                case OsType.Mac:
                    _knimeApp = "knime.app/Contents/MacOS/knime";
                    break;
                case OsType.Windows:
                    _knimeApp = "knime.exe";
                    break;
                case OsType.X11:
                    _knimeApp = "knime";
                    break;
                case OsType.Other:
                    throw new NotImplementedException("Could not determine operating system type");
                default:
                    throw new ArgumentOutOfRangeException();
            }

            /* Empty dir means using path so skip checking file existance. */
            if (_knimeDir != null && !File.Exists(_knimeDir + _knimeApp))
                throw new FileNotFoundException("The KNIME executable could not be found");
        }

        /// <summary>
        /// Calls the KNIME instance with the parameters given.
        /// </summary>
        /// <param name="workDir">Sets the working directory for the process to be started. If left empty this defaults to %SYSTEMROOT%\system32</param>
        /// <param name="output">Collects the redirected output messages.</param>
        /// <param name="error">Collects the redirected error messages.</param>
        /// <returns>An instance of <see cref="ExitStatus"/></returns>
        /// <exception cref="System.NullReferenceException">KNIME either exited too fast or could not get started at all.</exception>
        [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
        public async Task<ExitStatus> StartKnime(string workDir = null, TextWriter output = null, TextWriter error = null)
        {
            var exitStatus = new ExitStatus();
            var filename = _knimeDir + _knimeApp;
            var arguments = Arguments.ToString();

            using (var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    CreateNoWindow = true,
                    Arguments = arguments,
                    FileName = filename,
                    RedirectStandardOutput = output != null,
                    RedirectStandardError = error != null,
                    UseShellExecute = false,
                }
            })
            {
                if (workDir != null)
                    process.StartInfo.WorkingDirectory = workDir;
                
                _executionCancellationTokenSource = Timeout.HasValue
                    ? new CancellationTokenSource(Timeout.Value * 1000)
                    : new CancellationTokenSource();

                try
                {
                    process.Start();

#pragma warning disable AsyncFixer04 // A disposable object used in a fire & forget async call
                    var tasks = new List<Task>(3) { process.WaitForExitAsync(_executionCancellationTokenSource.Token) };
#pragma warning restore AsyncFixer04 // A disposable object used in a fire & forget async call
                    if (output != null)
                    {
                        tasks.Add(ActionExtensions.ReadAsync(drh =>
                        {
                            process.OutputDataReceived += drh;
                            process.BeginOutputReadLine();
                        }, drh => process.OutputDataReceived -= drh, output, _executionCancellationTokenSource.Token));
                    }

                    if (error != null)
                    {
                        tasks.Add(ActionExtensions.ReadAsync(drh =>
                        {
                            process.ErrorDataReceived += drh;
                            process.BeginErrorReadLine();
                        }, drh => process.ErrorDataReceived -= drh, error, _executionCancellationTokenSource.Token));
                    }

                    if (process.HasExited || process.Id == 0)
                        throw new NullReferenceException("KNIME either exited too fast or could not get started at all.");

                    await Task.WhenAll(tasks);
                    exitStatus.ExitCode = process.ExitCode;
                }
                catch (NullReferenceException ex)
                {
                    exitStatus.LastErrorMessage = ex.Message;
                }
                /* This method timeouted, not knime loader */
                catch (TaskCanceledException)
                {
                    exitStatus.LastErrorMessage = "KNIME process was cancelled.";
                }
                /* should not appear */
                catch (ObjectDisposedException)
                {
                    exitStatus.LastErrorMessage = "KNIME process ended preliminary.";
                }
                finally
                {
                    if (!string.IsNullOrEmpty(exitStatus.LastErrorMessage) && KillOnError)
                    {
                        try
                        {
                            process.Kill();
                            exitStatus.KilledProcess = true;
                            exitStatus.ExitCode = -1; // successful cancellation otherwise sets exit code 0
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }

                return exitStatus;
            }
        }

        /// <summary>
        /// Cancels a pending KNIME execution.
        /// </summary>
        public void AbortKnime()
        {
            _executionCancellationTokenSource.Cancel();
        }
    }
}