using System;
using System.IO;
using System.Text;
using KnimeNet.CommandLine.Types.Args;

namespace KnimeNet.CommandLine.Types
{
    /// <summary>
    /// Like a <see cref="TextWriter"/> but instead of writing internally every write attempt triggers an event.
    /// </summary>
    public class EventBasedTextWriter : TextWriter
    {
        /// <summary>
        /// Called when the <see cref="TextWriter"/>s Write method is called.
        /// </summary>
        public event EventHandler<TextWriterEventArgs> OnWrite;
        /// <summary>
        /// Called when the <see cref="TextWriter"/>s WriteLine method is called.
        /// </summary>
        public event EventHandler<TextWriterEventArgs> OnWriteLine;

        /// <summary>
        /// Change encoding to UTF8 as standard.
        /// </summary>
        public override Encoding Encoding => Encoding.UTF8;

        /// <summary>
        /// Calls the <see cref="OnWrite"/>
        /// </summary>
        /// <param name="value">The string to write.</param>
        public override void Write(string value)
        {
            OnWrite?.Invoke(this, new TextWriterEventArgs(value));
        }

        /// <summary>
        /// Calls the <see cref="OnWriteLine"/>
        /// </summary>
        /// <param name="value">The string to write.</param>
        public override void WriteLine(string value)
        {
            OnWriteLine?.Invoke(this, new TextWriterEventArgs(value));
        }
    }
}
