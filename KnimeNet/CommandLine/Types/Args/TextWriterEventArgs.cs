using System;

namespace KnimeNet.CommandLine.Types.Args
{
    /// <summary>
    /// <see cref="EventArgs"/> containing an <see cref="string"/>
    /// </summary>
    /// <seealso cref="EventArgs" />
    public class TextWriterEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public string Data { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextWriterEventArgs"/> class.
        /// </summary>
        /// <param name="data">The data stream.</param>
        internal TextWriterEventArgs(string data)
        {
            Data = data;
        }
    }

}
