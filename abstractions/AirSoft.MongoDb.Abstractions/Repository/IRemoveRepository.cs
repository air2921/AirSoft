using AirSoft.Exceptions;
using AirSoft.MongoDb.Abstractions.Builders.State.Remove;
using AirSoft.MongoDb.Abstractions.Documents;

namespace AirSoft.MongoDb.Abstractions.Repository
{
    /// <summary>
    /// Provides repository pattern operations for removing documents from the data store.
    /// Supports both single and batch removal operations through builder pattern configuration.
    /// </summary>
    /// <typeparam name="TDocument">The type of document the repository will handle, which must inherit from <see cref="DocumentBase"/>.</typeparam>
    public interface IRemoveRepository<TDocument> where TDocument : DocumentBase
    {
        /// <summary>
        /// Asynchronously removes an document using a configured builder.
        /// </summary>
        /// <param name="builder">Configured remove builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the number of removed documents (1 if successful, 0 otherwise).</returns>
        /// <exception cref="documentsException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<int> RemoveAsync(RemoveSingleDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously removes an document by configuring the remove builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the remove builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the number of removed documents (1 if successful, 0 otherwise).</returns>
        /// <exception cref="DocumentException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<int> RemoveAsync(Action<RemoveSingleDocumentBuilder<TDocument>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes an document using a configured builder.
        /// </summary>
        /// <param name="builder">Configured remove builder</param>
        /// <returns>The number of removed documents (1 if successful, 0 otherwise).</returns>
        /// <exception cref="DocumentException">Thrown when database operation fails</exception>
        public int Remove(RemoveSingleDocumentBuilder<TDocument> builder);

        /// <summary>
        /// Removes an document by configuring the remove builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the remove builder</param>
        /// <returns>The number of removed documents (1 if successful, 0 otherwise).</returns>
        /// <exception cref="DocumentException">Thrown when database operation fails</exception>
        public int Remove(Action<RemoveSingleDocumentBuilder<TDocument>> builderAction);

        /// <summary>
        /// Asynchronously removes multiple documents using a configured builder.
        /// </summary>
        /// <param name="builder">Configured remove builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the number of removed documents.</returns>
        /// <exception cref="DocumentException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<int> RemoveRangeAsync(RemoveRangeDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously removes multiple documents by configuring the remove builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the remove builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the number of removed documents.</returns>
        /// <exception cref="DocumentException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<int> RemoveRangeAsync(Action<RemoveRangeDocumentBuilder<TDocument>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes multiple documents using a configured builder.
        /// </summary>
        /// <param name="builder">Configured remove builder</param>
        /// <returns>The number of removed documents.</returns>
        /// <exception cref="DocumentException">Thrown when database operation fails</exception>
        public int RemoveRange(RemoveRangeDocumentBuilder<TDocument> builder);

        /// <summary>
        /// Removes multiple documents by configuring the remove builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the remove builder</param>
        /// <returns>The number of removed documents.</returns>
        /// <exception cref="DocumentException">Thrown when database operation fails</exception>
        public int RemoveRange(Action<RemoveRangeDocumentBuilder<TDocument>> builderAction);
    }
}
