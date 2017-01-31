using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using KnimeNet.CommandLine.Types;
using KnimeNet.CommandLine.Types.Attributes;
using KnimeNet.CommandLine.Types.Enums;
using NUnit.Framework;

namespace KnimeNet.Test
{
    [TestFixture]
    public class TestCommandLine
    {
        private readonly ArgumentBag _bag = new ArgumentBag();

        private readonly WorkFlowVariable _v1 = new WorkFlowVariable("name", "1", VariableType.String);
        private readonly WorkFlowVariable _v2 = new WorkFlowVariable("name", "1", VariableType.Integer);
        private readonly WorkFlowVariable _v3 = new WorkFlowVariable("name", "1", VariableType.Double);

        private readonly MasterKey _mk = new MasterKey {Key = "keyphrase"};

        private readonly Credential _c1 = new Credential("cred");
        private readonly Credential _c2 = new Credential("2ndCred") { Login = "login" };
        private readonly Credential _c3 = new Credential("3rdCred") { Login = "login", Password = "pass" };
        private readonly Credential _c4 = new Credential("4thCred") { Password = "pass" };

        private readonly VmArgument _vm1 = new VmArgument("key1", "value1");
        private readonly VmArgument _vm2 = new VmArgument("key2", "value2");

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            /* create minimalistic bag with no defaults
             * flags with default true -> false
             * empty all other defaults */
            _bag.Application = null;
            _bag.NoSplash = false;
            _bag.NoSave = false;
            _bag.Reset = false;
            _bag.SuppressErrors = false;
        }

        [Test, Category("Command Line ArgumentBag")]
        public void CreateAsOneLiner()
        {
            var argumentBag = new ArgumentBag("-reset");
            Assert.That(argumentBag.ToString() == "-reset");
        }

        [Test, Category("Command Line ArgumentBag")]
        public void FullSerializationAndDeserialization()
        {
            var argumentBag = ArgumentBags.FullArgumentBag;
            var json = argumentBag.ToJson(true);
            var deserializedArgumentBag = ArgumentBag.FromJson(json);
            Assert.IsNotNull(deserializedArgumentBag);
        }

        [Test, Category("Command Line ArgumentBag")]
        public void MinimalSerializationAndDeserialization()
        {
            var argumentBag = ArgumentBags.MinArgumentBag;
            var json = argumentBag.ToJson(true);
            var deserializedArgumentBag = ArgumentBag.FromJson(json);
            Assert.IsNotNull(deserializedArgumentBag);
        }

        [Test, Category("Command Line ArgumentBag")]
        public void ArgumentBagToString()
        {
            /* single flag */
            _bag.NoSplash = true;
            Assert.AreEqual(_bag.Prefix(nameof(_bag.NoSplash)), _bag.ToString());
            /* second flag and non-flag */
            _bag.ConsoleLog = true;
            _bag.Application = "app";
            /* two workflow variables */
            _bag.WorkFlowVariables = new[] {_v1, _v2};
            /* a masterkey */
            _bag.MasterKey = _mk;
            /* two credentials */
            _bag.Credentials = new[] {_c1, _c2};
            /* two vm arguments */
            _bag.VmArguments = new[] {_vm1, _vm2};
            var args = _bag.ToString();
            args = Regex.Replace(args, _bag.Prefix(nameof(_bag.NoSplash)) + @"\s?", "");
            args = Regex.Replace(args, _bag.Prefix(nameof(_bag.ConsoleLog)) + @"\s?", "");
            args = Regex.Replace(args, _bag.Prefix(nameof(_bag.Application)) + @"app\s?", "");
            args = Regex.Replace(args, _bag.Prefix(nameof(_bag.WorkFlowVariables)) + _v1 + @"\s?", "");
            args = Regex.Replace(args, _bag.Prefix(nameof(_bag.WorkFlowVariables)) + _v2 + @"\s?", "");
            args = Regex.Replace(args, _bag.Prefix(nameof(_bag.MasterKey)) + _mk + @"\s?", "");
            args = Regex.Replace(args, _bag.Prefix(nameof(_bag.Credentials)) + _c1 + @"\s?", "");
            args = Regex.Replace(args, _bag.Prefix(nameof(_bag.Credentials)) + _c2 + @"\s?", "");
            args = Regex.Replace(args, _bag.Prefix(nameof(_bag.VmArguments)) + _vm1 + @"\s?", "");
            /* without repeating the key -> no prefix */
            args = Regex.Replace(args, _vm2 + @"\s?", "");
            Assert.AreEqual(args, "");
        }

        [Test, Category("Command Line WorkFlowVariable")]
        public void WorkFlowVariableToString()
        {
            Assert.That(_v1.ToString() == "name,1,String");
            Assert.That(_v2.ToString() == "name,1,int");
            Assert.That(_v3.ToString() == "name,1,double");
        }

        [Test, Category("Command Line Credential")]
        public void CredentialToString()
        {
            Assert.That(_c1.ToString() == "cred");
            Assert.That(_c2.ToString() == "2ndCred;login");
            Assert.That(_c3.ToString() == "3rdCred;login;pass");
            Assert.That(_c4.ToString() == "4thCred");
        }

        [Test, Category("Command Line VmArgument")]
        public void VmArgumentToString()
        {
            Assert.That(_vm1.ToString() == "key1=value1");
            Assert.That(_vm2.ToString() == "key2=value2");
        }
    }

    internal static class ArgumentBagExtension
    {
        internal static string Prefix(this ArgumentBag bag, string property)
        {
            var info = bag.GetType().GetProperty(property);
            var claAttribute =
                    (CommandLineArgumentAttribute)info.GetCustomAttribute(typeof(CommandLineArgumentAttribute));
            if (claAttribute == null) return string.Empty;
            return claAttribute.Name + claAttribute.Seperator.GetType()
                .GetRuntimeField(claAttribute.Seperator.ToString())
                .GetCustomAttributes(typeof(EnumMemberAttribute), true)
                .Select(a => ((EnumMemberAttribute)a).Value).FirstOrDefault();
        }
    }
}
