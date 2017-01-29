using Newtonsoft.Json;

namespace KnimeNet.CommandLine.Types
{
    /// <summary>
    /// Represents a generic VM argument.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class VmArgument
    {
        /// <summary>
        /// The key of the argument e.g. eclipse.xyz without '-D' prefix
        /// </summary>
        [JsonProperty("key", Required = Required.Always)]
        public string Key { get; set; }

        /// <summary>
        /// The value of the argument.
        /// </summary>
        [JsonProperty("value", Required = Required.Always)]
        public string Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VmArgument"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public VmArgument(string key, string value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"-D{Key}={Value}";
        }
    }
}
