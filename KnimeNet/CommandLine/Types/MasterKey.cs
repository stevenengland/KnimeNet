using Newtonsoft.Json;

namespace KnimeNet.CommandLine.Types
{
    /// <summary>
    /// Represents a KNIME workflow master key.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class MasterKey
    {
        /// <summary>
        /// The masterkey. Will prompt if not provided.
        /// </summary>
        /// <remarks>Leaving this value empty is not useful in unattended situations</remarks>
        [JsonProperty("key", Required = Required.Always)]
        public string Key { get; set; }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{Key}";
        }
    }
}
