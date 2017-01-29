using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace KnimeNet.CommandLine.Types.Enums
{
    /// <summary>
    /// Represents the way argument keys are seperated from the argument values.
    /// Example: key value or key=value
    /// </summary>
    public enum ArgumentSeperator
    {
        /// <summary>
        /// Seperation via whitespace
        /// </summary>
        [EnumMember(Value = " ")]
        Space,

        /// <summary>
        /// Seperation via equals sign
        /// </summary>
        [EnumMember(Value = "=")]
        Equals,
        /// <summary>
        /// No seperation at all.
        /// </summary>
        [EnumMember(Value = "")]
        None
    }

    internal static class ArgumentSeperatorExtensions
    {
        /// <summary>
        /// Gets the enum mebmber value of <see cref="ArgumentSeperator"/>.
        /// </summary>
        /// <param name="seperator">The seperator.</param>
        /// <returns></returns>
        internal static string ToArgumentSeperatorString(this ArgumentSeperator seperator)
        {
            return seperator.GetType()
                .GetRuntimeField(seperator.ToString())
                .GetCustomAttributes(typeof(EnumMemberAttribute), true)
                .Select(a => ((EnumMemberAttribute)a).Value).FirstOrDefault();
        }
    }
}
