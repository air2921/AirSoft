using AirSoft.Exceptions.Base;

namespace AirSoft.Communication.Abstractions
{
    /// <summary>
    /// Interface for message senders.
    /// </summary>
    /// <typeparam name="TMessage">The type of message, must be <see cref="IMessage"/>.</typeparam>
    public interface ISender<in TMessage> where TMessage : IMessage
    {
        /// <summary>
        /// Asynchronously sends a message.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="SenderException">Thrown when an error occurs during sending message or cancel operation</exception>
        public Task SendAsync(TMessage message, CancellationToken cancellationToken = default);

        /// <summary>
        /// Synchronously sends a message.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        /// <exception cref="SenderException">Thrown when an error occurs during sending message</exception>
        public void Send(TMessage message);
    }
}
