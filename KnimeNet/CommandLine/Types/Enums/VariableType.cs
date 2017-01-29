using System.Reflection;
using System.Runtime.Serialization;
using System.Linq;

namespace KnimeNet.CommandLine.Types.Enums
{
    /// <summary>
    /// Represents the type of the variable
    /// </summary>
    public enum VariableType
    {
        /// <summary>
        /// The variable is of type string.
        /// </summary>
        [EnumMember(Value = "String")]
        String,
        /// <summary>
        /// The variable is of type integer.
        /// </summary>
        [EnumMember(Value = "int")]
        Integer,
        /// <summary>
        /// The variable is of type double.
        /// </summary>
        [EnumMember(Value = "double")]
        Double
    }

    internal static class ParameterTypeExtensions
    {
        /// <summary>
        /// Gets the enum member value of <see cref="VariableType"/>
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        internal static string ToParameterTypeString(this VariableType type)
        {
            return type.GetType()
                .GetRuntimeField(type.ToString())
                .GetCustomAttributes(typeof(EnumMemberAttribute), true)
                .Select(a => ((EnumMemberAttribute)a).Value).FirstOrDefault();
        }
    }
}
