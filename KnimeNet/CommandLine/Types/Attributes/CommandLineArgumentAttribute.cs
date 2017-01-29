using System;
using KnimeNet.CommandLine.Types.Enums;

namespace KnimeNet.CommandLine.Types.Attributes
{

    /// <summary>
    /// Attribute specific for signalling a command line argument
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property)]
    public class CommandLineArgumentAttribute : Attribute
    {
        /// <summary>
        /// Specifies how the argument key is seperated from the value.
        /// </summary>
        public ArgumentSeperator Seperator { get; } = ArgumentSeperator.None;

        /// <summary>
        /// Indicates, if the argument is a flag. Flags are handled as keys but have no values.
        /// </summary>
        public bool IsFlag { get; }

        /// <summary>
        /// Get the argument name.
        /// </summary>
        public string Name { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineArgumentAttribute"/> class.
        /// </summary>
        /// <param name="name">The command line key</param>
        /// <param name="seperator">Sets the key/value seperator</param>
        public CommandLineArgumentAttribute(string name, ArgumentSeperator seperator)
        {
            Name = name;
            Seperator = seperator;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineArgumentAttribute"/> class.
        /// This attribute is implicitely a flag. That means there is no value vor that key.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        public CommandLineArgumentAttribute(string name)
        {
            Name = name;
            IsFlag = true;
        }
    }
}