namespace AirSoft.Communication.Abstractions
{
    /// <summary>
    /// Interface for message senders.
    /// </summary>
    /// <typeparam name="TMessage">The type of message, must be a class.</typeparam>
    public interface ISender<TMessage> where TMessage : class
    {
        /// <summary>
        /// Asynchronously sends a message.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task SendAsync(TMessage message, CancellationToken cancellationToken = default);

        /// <summary>
        /// Synchronously sends a message.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        public void Send(TMessage message);
    }
}
