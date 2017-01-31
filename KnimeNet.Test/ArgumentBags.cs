using KnimeNet.CommandLine.Types;
using KnimeNet.CommandLine.Types.Enums;

namespace KnimeNet.Test
{
    public static class ArgumentBags
    {
        public const string AppArgValue = "TestApplication";

        public static readonly ArgumentBag FullArgumentBag = new ArgumentBag()
        {
            Application = AppArgValue,
            ConsoleLog = false,
            NoExit = false,
            NoSplash = false,
            NoSave = false,
            Reset = false,
            FailOnLoadError = false,
            MasterKey = new MasterKey {Key = "key1234"},
            Credentials = new []
            {
                new Credential("c1"),
                new Credential("c1") {Login = "login", Password = "pass"}  
            },
            WorkFlowVariables = new[]
            {
                new WorkFlowVariable ("var1", "value1", VariableType.Double),
                new WorkFlowVariable ("var2", "value2", VariableType.Integer),
                new WorkFlowVariable ("var3", "value3", VariableType.String)
            },
            VmArguments = new []
            {
              new VmArgument("key1", "value1"),
              new VmArgument("key2", "value2"),   
            },
            Arch = "x86",
            Clean = false,
            Data = "pathtodata",
            LauncherAppendVmArgs = false,
            LauncherIni = "pathtoini",
            LauncherLibrary = "pathtolib",
            LauncherTimeout = 30,
            SuppressErrors = false,
            UpdateLinks = false,
            Preferences = "pathtopreferencesfile",
            WorkFlowDir = "pathtoworkflow",
            WorkFlowFile = "pathtoworkflowfile",
            DestDir = "pathtodest",
            DestFile = "pathtodestfile"
        };

        public static readonly ArgumentBag MinArgumentBag = new ArgumentBag();
    }
}