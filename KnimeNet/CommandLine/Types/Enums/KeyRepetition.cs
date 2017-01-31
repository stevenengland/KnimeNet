namespace KnimeNet.CommandLine.Types.Enums
{
    /// <summary>
    /// Describes in case of IEnumerable arguments how to repeat the argument's key
    /// </summary>
    public enum KeyRepetition
    {
        /// <summary>
        /// The key should not be written at all.
        /// </summary>
        None,
        /// <summary>
        /// The key should only be written one single time.
        /// </summary>
        Single,
        /// <summary>
        /// For key should be written for each value.
        /// </summary>
        ForEach
    }
}
