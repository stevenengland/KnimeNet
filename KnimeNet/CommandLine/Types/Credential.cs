using Newtonsoft.Json;

namespace KnimeNet.CommandLine.Types
{
    /// <summary>
    /// Represents a KNIME workflow credential
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Credential
    {
        /// <summary>
        /// The credential identifier.
        /// </summary>
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        /// <summary>
        /// The users login name. Leave emtpy to be prompted for. 
        /// </summary>
        /// <remarks>Leaving this value empty is not useful in unattended situations</remarks>
        [JsonProperty("login", Required = Required.Default)]
        public string Login { get; set; }

        /// <summary>
        /// The users password. Leave emtpy to be prompted for.
        /// </summary>
        /// <remarks>Leaving this value empty is not useful in unattended situations</remarks>
        [JsonProperty("password", Required = Required.Default)]
        public string Password { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Credential"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public Credential(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var credential = Name;
            if (string.IsNullOrEmpty(Login)) return credential;
            credential += $";{Login}";
            if (!string.IsNullOrEmpty(Password))
                credential += $";{Password}";
            return credential;
        }
    }
}
