using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace KnimeNet.Extensions
{
    /// <summary>
    /// Extensions for <see cref="Action"/>s.
    /// </summary>
    public static class ActionExtensions
    {
        /// <summary>
        /// Reads the data from the specified data recieved event and writes it to the
        /// <paramref name="textWriter" />.
        /// </summary>
        /// <param name="addHandler">Adds the event handler.</param>
        /// <param name="removeHandler">Removes the event handler.</param>
        /// <param name="textWriter">The text writer.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public static Task ReadAsync(
            this Action<DataReceivedEventHandler> addHandler,
            Action<DataReceivedEventHandler> removeHandler,
            TextWriter textWriter,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var taskCompletionSource = new TaskCompletionSource<object>();

            DataReceivedEventHandler handler = null;
            handler = (sender, e) =>
            {
                if (e.Data == null)
                {
                    removeHandler(handler);
                    taskCompletionSource.TrySetResult(null);
                }
                else
                {
                    textWriter.WriteLine(e.Data);
                }
            };

            addHandler(handler);

            if (cancellationToken != default(CancellationToken))
            {
                cancellationToken.Register(
                    () =>
                    {
                        removeHandler(handler);
                        taskCompletionSource.TrySetCanceled();
                    });
            }

            return taskCompletionSource.Task;
        }
    }
}