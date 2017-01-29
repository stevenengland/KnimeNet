using KnimeNet.CommandLine.Types.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KnimeNet.CommandLine.Types
{
    /// <summary>
    /// Represents a KNIME workflow variable.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class WorkFlowVariable
    {
        /// <summary>
        /// The variable identifier.
        /// </summary>
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        /// <summary>
        /// The varaible value.
        /// </summary>
        [JsonProperty("value", Required = Required.Always)]
        public string Value { get; set; }

        /// <summary>
        /// The type of the variable.
        /// </summary>
        [JsonProperty("type", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public VariableType Type { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkFlowVariable"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="variableType">Type of the variable.</param>
        public WorkFlowVariable(string name, string value, VariableType variableType)
        {
            Name = name;
            Value = value;
            Type = variableType;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{Name},{Value},{Type.ToParameterTypeString()}";
        }
    }
}
