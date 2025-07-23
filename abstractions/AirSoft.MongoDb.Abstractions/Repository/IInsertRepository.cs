using AirSoft.Exceptions;
using AirSoft.MongoDb.Abstractions.Builders.State.Insert;
using AirSoft.MongoDb.Abstractions.Documents;

namespace AirSoft.MongoDb.Abstractions.Repository
{
    /// <summary>
    /// Provides repository pattern operations for inserting documents to the data store.
    /// Supports both single and batch operations through builder pattern configuration.
    /// </summary>
    /// <typeparam name="TDocument">The type of document the repository will handle, which must inherit from <see cref="DocumentBase"/>.</typeparam>
    public interface IInsertRepository<TDocument> where TDocument : DocumentBase
    {
        /// <summary>
        /// Asynchronously inserts an document using a configured builder.
        /// </summary>
        /// <param name="builder">Configured insert builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the number of added documents (1 if successful, 0 otherwise).</returns>
        /// <exception cref="DocumentException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<long> InsertAsync(InsertSingleDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously inserts an document by configuring the builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the insert builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the number of added documents (1 if successful, 0 otherwise).</returns>
        /// <exception cref="DocumentException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<long> InsertAsync(Action<InsertSingleDocumentBuilder<TDocument>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Inserts an document using a configured builder.
        /// </summary>
        /// <param name="builder">Configured insert builder</param>
        /// <returns>The number of added documents (1 if successful, 0 otherwise).</returns>
        /// <exception cref="DocumentException">Thrown when database operation fails</exception>
        public long Insert(InsertSingleDocumentBuilder<TDocument> builder);

        /// <summary>
        /// Inserts an document by configuring the builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the insert builder</param>
        /// <returns>The number of added documents (1 if successful, 0 otherwise).</returns>
        /// <exception cref="DocumentException">Thrown when database operation fails</exception>
        public long Insert(Action<InsertSingleDocumentBuilder<TDocument>> builderAction);

        /// <summary>
        /// Asynchronously inserts multiple documents using a configured builder.
        /// </summary>
        /// <param name="builder">Configured insert builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the number of added documents.</returns>
        /// <exception cref="DocumentException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<long> InsertRangeAsync(InsertRangeDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously inserts multiple documents  by configuring the builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the insert builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the number of added documents.</returns>
        /// <exception cref="DocumentException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<long> InsertRangeAsync(Action<InsertRangeDocumentBuilder<TDocument>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Inserts multiple documents using a configured builder.
        /// </summary>
        /// <param name="builder">Configured insert builder</param>
        /// <returns>The number of added documents.</returns>
        /// <exception cref="DocumentException">Thrown when database operation fails</exception>
        public long InsertRange(InsertRangeDocumentBuilder<TDocument> builder);

        /// <summary>
        /// Inserts multiple documents by configuring the builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the insert builder</param>
        /// <returns>The number of added documents.</returns>
        /// <exception cref="DocumentException">Thrown when database operation fails</exception>
        public long InsertRange(Action<InsertRangeDocumentBuilder<TDocument>> builderAction);
    }
}
