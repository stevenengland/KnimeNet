namespace KnimeNet.Extensions
{
    /// <summary>
    /// Extensions for strings.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Conactenates a given argument string with a new argument key and value.
        /// </summary>
        /// <param name="oldArguments">The existing argument string.</param>
        /// <param name="argumentKey">The key of the new argument.</param>
        /// <param name="argumentValue">The value of the new argument.</param>
        /// <param name="seperator">The separtor between key and value.</param>
        /// <returns>The combined old an new arguments</returns>
        public static string CombineArguments(this string oldArguments, string argumentKey, string argumentValue, string seperator)
        {
            if (string.IsNullOrEmpty(argumentKey))
                return oldArguments;
            if (string.IsNullOrEmpty(oldArguments))
                oldArguments += string.IsNullOrEmpty(argumentValue) ? $"{argumentKey}" : $"{argumentKey}{seperator}{argumentValue}";
            else
                oldArguments += string.IsNullOrEmpty(argumentValue) ? $" {argumentKey}" : $" {argumentKey}{seperator}{argumentValue}";
            return oldArguments;
        }
    }
}
