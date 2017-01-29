namespace KnimeNet.CommandLine.Types
{
    /// <summary>
    /// A collection of information concerning the KNIME execution.
    /// </summary>
    public class ExitStatus
    {
        /// <summary>
        /// If an error occured in <see cref="ShellProxy"/> this variable contains more information.
        /// It is not intended to record KNIME errors. These are captured e.g. via <see cref="EventBasedTextWriter"/>s.
        /// </summary>
        public string LastErrorMessage { get; internal set; } = string.Empty;

        /// <summary>
        /// Signals if the started process was killed. In case the started process should be killed in case of an error the <see cref="ShellProxy"/> needs to be instructed to do so.
        /// </summary>
        public bool KilledProcess { get; internal set; }

        /// <summary>
        /// Represents the exit code produced by KNIME.
        /// </summary>
        public int ExitCode { get; internal set; }
    }
}
