using AirSoft.Exceptions;
using AirSoft.MongoDb.Abstractions.Builders.State.Replace;
using AirSoft.MongoDb.Abstractions.Documents;

namespace AirSoft.MongoDb.Abstractions.Repository
{
    public interface IReplaceRepository<TDocument> where TDocument : DocumentBase
    {
        /// <summary>
        /// Asynchronously replace an document using a configured builder.
        /// </summary>
        /// <param name="builder">Configured replace builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the number of replaced documents (1 if successful, 0 otherwise).</returns>
        /// <exception cref="documentsException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<int> ReplaceAsync(ReplaceSingleDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously replace an document by configuring the replace builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the replace builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the number of replaced documents (1 if successful, 0 otherwise).</returns>
        /// <exception cref="DocumentException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<int> ReplaceAsync(Action<ReplaceSingleDocumentBuilder<TDocument>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Replace an document using a configured builder.
        /// </summary>
        /// <param name="builder">Configured replace builder</param>
        /// <returns>The number of replaced documents (1 if successful, 0 otherwise).</returns>
        /// <exception cref="DocumentException">Thrown when database operation fails</exception>
        public int Replace(ReplaceSingleDocumentBuilder<TDocument> builder);

        /// <summary>
        /// Replace an document by configuring the replace builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the replace builder</param>
        /// <returns>The number of replaced documents (1 if successful, 0 otherwise).</returns>
        /// <exception cref="DocumentException">Thrown when database operation fails</exception>
        public int Replace(Action<ReplaceSingleDocumentBuilder<TDocument>> builderAction);

        /// <summary>
        /// Asynchronously replace multiple documents using a configured builder.
        /// </summary>
        /// <param name="builder">Configured replace builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the number of replaced documents.</returns>
        /// <exception cref="DocumentException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<int> ReplaceRangeAsync(ReplaceRangeDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously replace multiple documents by configuring the replace builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the replace builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the number of replaced documents.</returns>
        /// <exception cref="DocumentException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<int> ReplaceRangeAsync(Action<ReplaceRangeDocumentBuilder<TDocument>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Replace multiple documents using a configured builder.
        /// </summary>
        /// <param name="builder">Configured replace builder</param>
        /// <returns>The number of replaced documents.</returns>
        /// <exception cref="DocumentException">Thrown when database operation fails</exception>
        public int ReplaceRange(ReplaceRangeDocumentBuilder<TDocument> builder);

        /// <summary>
        /// Replace multiple documents by configuring the replace builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the replace builder</param>
        /// <returns>The number of replaced documents.</returns>
        /// <exception cref="DocumentException">Thrown when database operation fails</exception>
        public int ReplaceRange(Action<ReplaceRangeDocumentBuilder<TDocument>> builderAction);
    }
}
