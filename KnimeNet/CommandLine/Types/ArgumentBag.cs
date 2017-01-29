using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using KnimeNet.CommandLine.Types.Attributes;
using KnimeNet.CommandLine.Types.Enums;
using KnimeNet.Environment.Types.Enums;
using KnimeNet.Extensions;
using Newtonsoft.Json;
using OperatingSystem = KnimeNet.Environment.OperatingSystem;

namespace KnimeNet.CommandLine.Types
{
    /// <summary>
    /// Description of ArgumentBag.
    /// http://help.eclipse.org/neon/index.jsp?topic=%2Forg.eclipse.platform.doc.isv%2Freference%2Fmisc%2Fruntime-options.html
    /// https://tech.knime.org/faq#q12
    /// If you pass no options, all available options will be listed.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class ArgumentBag
    {
        #region Command Line Constants
         
        private const string ArgNoSave = "-nosave";
        private const string ArgReset = "-reset";    
        private const string ArgFailOnLoadError = "-failonloaderror";
        private const string ArgUpdateLinks = "-updateLinks";
        private const string ArgWorkFlowVar = "-workflow.variable";
        private const string ArgCredential = "-credential";
        private const string ArgMasterKey = "-masterkey";
        private const string ArgPreferences = "-preferences";
        private const string ArgWorkFlowFile = "-workflowFile";
        private const string ArgWorkFlowDir = "-workflowDir";
        private const string ArgDestFile = "-destFile";
        private const string ArgDestDir = "-destDir";

        private const string ArgApplication = "-application";
        private const string ArgNoExit = "-noexit";
        private const string ArgNoSplash = "-nosplash";
        private const string ArgConsoleLog = "-consoleLog";
        private const string ArgArch = "-arch";
        private const string ArgClean = "-clean";
        private const string ArgData = "-data";
        private const string ArgLauncherLibrary = "--launcher.library";
        private const string ArgLauncherIni = "--launcher.ini";
        private const string ArgLauncherSuppressErrors = "--launcher.suppressErrors";
        private const string ArgLauncherTimeout = "--launcher.timeout";
        private const string ArgLauncherAppendVmArgs = "--launcher.appendVmargs";

        private const string ArgVmArgs = "-vmargs";


        #endregion

        /// <summary>
        /// Indicates if the properties or a single string should be used to create the argument line.
        /// </summary>
        [JsonIgnore] private readonly bool _useProperties = true;
        /// <summary>
        /// The argument line that is used if properties shouldn't be used for creating an argument line.
        /// </summary>
        [JsonIgnore] private readonly string _argumentLine = "";

        #region Knime Native Commands

        /// <summary>
        /// If this is specified, the workflow is not saved after execution has finished.
        /// </summary>
        [JsonProperty(ArgNoSave, Required = Required.Default)]
        [CommandLineArgument(ArgNoSave)]
        public bool NoSave { get; set; } = true;

        /// <summary>
        /// Reset workflow prior to execution.
        /// </summary>
        [JsonProperty(ArgReset, Required = Required.Default)]
        [CommandLineArgument(ArgReset)]
        public bool Reset { get; set; } = true;

        /// <summary>
        /// Don't execute if there are errors during workflow loading
        /// </summary>
        [JsonProperty(ArgFailOnLoadError, Required = Required.Default)]
        [CommandLineArgument(ArgFailOnLoadError)]
        public bool FailOnLoadError { get; set; }

        /// <summary>
        /// Update metanode links to the latest version
        /// </summary>
        [JsonProperty(ArgUpdateLinks, Required = Required.Default)]
        [CommandLineArgument(ArgUpdateLinks)]
        public bool UpdateLinks { get; set; }

        /// <summary>
        /// Credentials to be used during the workflow.
        /// </summary>
        [JsonProperty(ArgCredential, Required = Required.Default)]
        [CommandLineArgument(ArgCredential, ArgumentSeperator.Equals)]
        public Credential[] Credentials { get; set; }

        /// <summary>
        /// Master password (used in e.g. database nodes).
        /// </summary>
        [JsonProperty(ArgMasterKey, Required = Required.Default)]
        [CommandLineArgument(ArgMasterKey, ArgumentSeperator.Equals)]
        public MasterKey MasterKey { get; set; }

        /// <summary>
        /// Path to the file containing Eclipse/KNIME preferences.
        /// </summary>
        [JsonProperty(ArgPreferences, Required = Required.Default)]
        [CommandLineArgument(ArgPreferences, ArgumentSeperator.Equals)]
        public string Preferences { get; set; }

        /// <summary>
        /// Zip file with a ready-to-execute workflow.
        /// </summary>
        [JsonProperty(ArgWorkFlowFile, Required = Required.Default)]
        [CommandLineArgument(ArgWorkFlowFile, ArgumentSeperator.Equals)]
        public string WorkFlowFile { get; set; }

        /// <summary>
        /// Directory with a ready-to-execute workflow.
        /// </summary>
        [JsonProperty(ArgWorkFlowDir, Required = Required.Default)]
        [CommandLineArgument(ArgWorkFlowDir, ArgumentSeperator.Equals)]
        public string WorkFlowDir { get; set; }

        /// <summary>
        /// Zip file where the executed workflow should be written to.
        /// If omitted the workflow is only saved in place.
        /// </summary>
        [JsonProperty(ArgDestFile, Required = Required.Default)]
        [CommandLineArgument(ArgDestFile, ArgumentSeperator.Equals)]
        public string DestFile { get; set; }

        /// <summary>
        /// Directory where the executed workflow is saved to. 
        /// If omitted the workflow is only saved in place.
        /// </summary>
        [JsonProperty(ArgDestDir, Required = Required.Default)]
        [CommandLineArgument(ArgDestDir, ArgumentSeperator.Equals)]
        public string DestDir { get; set; }

        /// <summary>
        /// Define or overwrite workflow variables.
        /// </summary>
        [JsonProperty(ArgWorkFlowVar, Required = Required.Default)]
        [CommandLineArgument(ArgWorkFlowVar, ArgumentSeperator.Equals)]
        public WorkFlowVariable[] WorkFlowVariables { get; set; }

        #endregion

        #region Eclipse Native Commands


        /// <summary>
        /// The identifier of the application to run. The value given here overrides any application defined by the product being run.
        /// In command line environments this is KNIME_BATCH_APPLICATION
        /// </summary>
        [JsonProperty(ArgApplication, Required = Required.Default)]
        [CommandLineArgument(ArgApplication, ArgumentSeperator.Space)]
        public string Application { get; set; } = "org.knime.product.KNIME_BATCH_APPLICATION";

        /// <summary>
        /// The processor architecture value. 
        /// The value should be one of the processor architecture names known to Eclipse (e.g., x86, ppc, sparc, ...). 
        /// See org.eclipse.osgi.service.environment.Constants for known values.
        /// </summary>
        [JsonProperty(ArgArch, Required = Required.Default)]
        [CommandLineArgument(ArgArch, ArgumentSeperator.Space)]
        public string Arch { get; set;}

        /// <summary>
        /// If set to "true", any cached data used by the OSGi framework and eclipse runtime will be wiped clean. 
        /// This will clean the caches used to store bundle dependency resolution and eclipse extension registry data. 
        /// Using this option will force eclipse to reinitialize these caches.
        /// </summary>
        [JsonProperty(ArgClean, Required = Required.Default)]
        [CommandLineArgument(ArgClean)]
        public bool Clean { get; set; }

        /// <summary>
        /// The instance data location for this session. Plug-ins use this location to store their data. 
        /// For example, the Resources plug-in uses this as the default location for projects (aka the workspace). 
        /// See the section on locations for more details.
        /// </summary>
        [JsonProperty(ArgData, Required = Required.Default)]
        [CommandLineArgument(ArgData, ArgumentSeperator.Space)]
        public string Data { get; set; }

        /// <summary>
        /// The location of the eclipse executable's companion shared library. 
        /// If not specified the executable looks in the plugins directory for the appropriate org.eclipse.equinox.launcher.[platform] fragment with the highest version and uses the shared library named eclipse_* inside.
        /// </summary>
        [JsonProperty(ArgLauncherLibrary, Required = Required.Default)]
        [CommandLineArgument(ArgLauncherLibrary, ArgumentSeperator.Space)]
        public string LauncherLibrary { get; set; }

        /// <summary>
        /// The location of the product .ini file to use.  
        /// If not specified the executable will look for a file beside the launcher with the same name and the extension .ini (i.e. eclipse.exe looks for eclipse.ini, product.exe looks for product.ini).
        /// </summary>
        [JsonProperty(ArgLauncherIni, Required = Required.Default)]
        [CommandLineArgument(ArgLauncherIni, ArgumentSeperator.Space)]
        public string LauncherIni { get; set; }

        /// <summary>
        /// If specified the executable will not display any error or message dialogs.
        /// This is useful if the executable is being used in an unattended situation.
        /// </summary>
        [JsonProperty(ArgLauncherSuppressErrors, Required = Required.Default)]
        [CommandLineArgument(ArgLauncherSuppressErrors)]
        public bool SuppressErrors { get; set; } = true;

        /// <summary>
        /// If specified, any VM arguments on the commandline will be appended to any VM arguments specified in the launcher .ini file. 
        /// Using this option is recommended in every launcher .ini file that specifies VM arguments, because the default behavior of overriding VM arguments can have unexpected side-effects.
        /// </summary>
        [JsonProperty(ArgLauncherAppendVmArgs, Required = Required.Default)]
        [CommandLineArgument(ArgLauncherAppendVmArgs)]
        public bool LauncherAppendVmArgs { get; set; }

        /// <summary>
        /// A timeout value for how long the launcher should spend trying to communicate with an already running eclipse before the launcher gives up and launches a new eclipse instance. 
        /// Default is 60 (seconds).
        /// </summary>
        [JsonProperty(ArgLauncherTimeout, Required = Required.Default)]
        [CommandLineArgument(ArgLauncherTimeout, ArgumentSeperator.Space)]
        public int? LauncherTimeout { get; set; }

        /// <summary>
        /// Controls whether or not the splash screen is shown.
        /// </summary>
        [JsonProperty(ArgNoSplash, Required = Required.Default)]
        [CommandLineArgument(ArgNoSplash)]
        public bool NoSplash { get; set; } = true;

        /// <summary>
        /// if "true", the OSGi Framework will not be shut down after the Eclipse application has ended. 
        /// This is useful for examining the OSGi Framework after the Eclipse application has ended.
        /// Note that the VM will terminate if no active non-daemon threads exists.
        /// <remarks>This option should be avoided in unattended runs since it blocks KNIME. You should provide an Timeout together with this option.</remarks>
        /// </summary>
        [JsonProperty(ArgNoExit, Required = Required.Default)]
        [CommandLineArgument(ArgNoExit)]
        public bool NoExit { get; set; }

        /// <summary>
        /// Causes a new window to be opened containing the log messages and will keep the window open after the execution has finished. 
        /// You will need to close the window manually and an error message is produced from the Java process which you can safely ignore. 
        /// <remarks>This option should be avoided in unattended runs since it blocks KNIME. You should provide an Timeout together with this option.</remarks>
        /// </summary>
        [JsonProperty(ArgConsoleLog, Required = Required.Default)]
        [CommandLineArgument(ArgConsoleLog)]
        public bool ConsoleLog { get; set; }


        /* !! MUST be the last serialized */
        /// <summary>
        /// Lists the VM arguments used to run Eclipse. This information is used to construct relaunch command lines.
        /// When passed to the Eclipse, this option is used to customize the operation of the Java VM to use to run Eclipse. 
        /// If specified, this option must come at the end of the command line. Even if not specified on the executable command line, the executable will automatically add the relevant arguments (including the class being launched) to the command line passed into Java using the -vmargs argument. Java Main then stores this value in eclipse.vmargs.
        /// </summary>
        [JsonProperty(ArgVmArgs, Required = Required.Default)]
        [CommandLineArgument(ArgVmArgs, ArgumentSeperator.Space)]
        public VmArgument[] VmArguments { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new <see cref="ArgumentBag" /> that uses the provided argument string instead of serialzed properties.
        /// </summary>
        /// <param name="arguments">The arguments as one liner.</param>
        public ArgumentBag(string arguments)
        {
            _useProperties = false;
            _argumentLine = arguments;
        }

        /// <summary>
        /// Creates a new <see cref="ArgumentBag" /> with default properties.
        /// </summary>
        /// <exception cref="NotImplementedException">The operating system is not supported.</exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public ArgumentBag()
        {
            /* Set OS specific must haves */
            switch (OperatingSystem.GetOsType())
            {
                case OsType.Mac:
                    NoSplash = true;
                    break;
                case OsType.Windows:
                    NoSplash = true;
                    break;
                case OsType.X11:
                    NoSplash = true;
                    break;
                case OsType.Other:
                    throw new NotImplementedException("The operating system is not supported.");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        /// <summary>
        /// Serializes this <see cref="ArgumentBag" /> into a string.
        /// </summary>
        /// <returns>
        /// An argument one liner
        /// </returns>
        public override string ToString()
        {
            if (_useProperties == false)
                return _argumentLine;

            var arguments = "";
            foreach (var info in GetType().GetProperties())
            {
                var claAttribute = (CommandLineArgumentAttribute) info.GetCustomAttribute(typeof(CommandLineArgumentAttribute));
                if (claAttribute == null) continue;

                var propVal = info.GetValue(this, null);

                /* IEnumerable properties */
                var enumerable = propVal as IEnumerable;
                if (enumerable != null && !(enumerable is string))
                {
                    foreach (var item in enumerable)
                    {
                        arguments = arguments.CombineArguments(claAttribute.Name, item.ToString(), claAttribute.Seperator.ToArgumentSeperatorString());
                    }
                }
                else
                {
                    /* non collections like primitives and null values */
                    if (propVal == null || (claAttribute.IsFlag && propVal is bool && !(bool) propVal)) continue; // Skip false flags
                    arguments = arguments.CombineArguments(claAttribute.Name, (claAttribute.IsFlag) ? string.Empty : propVal.ToString(), claAttribute.Seperator.ToArgumentSeperatorString());
                }
            }
            return arguments;
        }

        /// <summary>
        /// Converts this <see cref="ArgumentBag" /> into a JSON formatted string.
        /// </summary>
        /// <param name="indended">Whether or not the output should be indended.</param>
        /// <returns>
        /// A JSON formatted string
        /// </returns>
        public string ToJson(bool indended = false)
        {
            return JsonConvert.SerializeObject(this, (indended) ? Formatting.Indented : Formatting.None);
        }

        /// <summary>
        /// Creates a new instance of <see cref="ArgumentBag" /> from a JSON string.
        /// </summary>
        /// <param name="json">The string to deserialze.</param>
        /// <returns>
        /// An <see cref="ArgumentBag" />.
        /// </returns>
        public static ArgumentBag FromJson(string json)
        {
            return JsonConvert.DeserializeObject<ArgumentBag>(json);
        }
    }
}