using System;
using System.Runtime.CompilerServices;
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
        /// For IEnumerable only. Specifies the way the key will be repeated. 
        /// </summary>
        public KeyRepetition Repetition { get; }

        /// <summary>
        /// Indicates, if the argument is a flag. Flags are handled as keys but have no values.
        /// </summary>
        public bool IsFlag { get; }

        /// <summary>
        /// Get the argument name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the order number.
        /// </summary>
        public int Order { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineArgumentAttribute"/> class.
        /// </summary>
        /// <param name="name">The command line key.</param>
        /// <param name="seperator">Sets the key/value seperator.</param>
        /// <param name="order">Sets the order for this argument</param>
        public CommandLineArgumentAttribute(string name, ArgumentSeperator seperator, [CallerLineNumber] int order = 0)
        {
            Name = name;
            Seperator = seperator;
            Repetition = KeyRepetition.ForEach;
            Order = order;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineArgumentAttribute"/> class.
        /// </summary>
        /// <param name="name">The command line key.</param>
        /// <param name="seperator">Sets the key/value seperator.</param>
        /// <param name="repetition">Sets the repitition type for the argument's key.</param>
        /// <param name="order">Sets the order for this argument</param>
        public CommandLineArgumentAttribute(string name, ArgumentSeperator seperator, KeyRepetition repetition, [CallerLineNumber] int order = 0)
        {
            Name = name;
            Seperator = seperator;
            Repetition = repetition;
            Order = order;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineArgumentAttribute"/> class.
        /// This attribute is implicitely a flag. That means there is no value vor that key.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="order">Sets the order for this argument</param>
        public CommandLineArgumentAttribute(string name, [CallerLineNumber] int order = 0)
        {
            Name = name;
            IsFlag = true;
            Order = order;
        }
    }
}