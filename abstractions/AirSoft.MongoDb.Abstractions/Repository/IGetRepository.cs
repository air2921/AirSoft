using AirSoft.Exceptions;
using AirSoft.MongoDb.Abstractions.Builders.Query;
using AirSoft.MongoDb.Abstractions.Details;
using AirSoft.MongoDb.Abstractions.Documents;
using MongoDB.Bson;
using System.Linq.Expressions;

namespace AirSoft.MongoDb.Abstractions.Repository
{
    /// <summary>
    /// Provides repository pattern operations for retrieving entities from the data store.
    /// Supports various query operations including single/multiple entity retrieval, counting, and paginated results.
    /// </summary>
    /// <typeparam name="TDocument">The type of document the repository will handle, which must inherit from <see cref="DocumentBase"/>.</typeparam>
    public interface IGetRepository<TDocument> where TDocument : DocumentBase
    {
        /// <summary>
        /// Asynchronously retrieves the count of documents that match the specified filter.
        /// </summary>
        /// <param name="filter">Optional filter expression</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the count of matching documents.</returns>
        /// <exception cref="DocumentException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<long> GetCountAsync(Expression<Func<TDocument, bool>>? filter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the count of documents that match the specified filter.
        /// </summary>
        /// <param name="filter">Optional filter expression</param>
        /// <returns>The count of matching documents.</returns>
        /// <exception cref="DocumentException">Thrown when database operation fails</exception>
        public long GetCount(Expression<Func<TDocument, bool>>? filter);

        /// <summary>
        /// Asynchronously retrieves an document by its identifier.
        /// </summary>
        /// <param name="id">document identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the found document or null.</returns>
        /// <exception cref="DocumentException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<TDocument?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves an document by its identifier.
        /// </summary>
        /// <param name="id">document identifier</param>
        /// <returns>The found document or null.</returns>
        /// <exception cref="DocumentException">Thrown when database operation fails</exception>
        public TDocument? GetById(ObjectId id);

        /// <summary>
        /// Asynchronously retrieves an document using a configured query builder.
        /// </summary>
        /// <param name="builder">Configured query builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the found document or null.</returns>
        /// <exception cref="DocumentException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<TDocument?> GetSingleAsync(SingleQueryDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves an document by configuring the query builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the query builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the found document or null.</returns>
        /// <exception cref="DocumentException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<TDocument?> GetSingleAsync(Action<SingleQueryDocumentBuilder<TDocument>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves an document using a configured query builder.
        /// </summary>
        /// <param name="builder">Configured query builder</param>
        /// <returns>The found document or null.</returns>
        /// <exception cref="DocumentException">Thrown when database operation fails</exception>
        public TDocument? GetSingle(SingleQueryDocumentBuilder<TDocument> builder);

        /// <summary>
        /// Retrieves an document by configuring the query builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the query builder</param>
        /// <returns>The found document or null.</returns>
        /// <exception cref="DocumentException">Thrown when database operation fails</exception>
        public TDocument? GetSingle(Action<SingleQueryDocumentBuilder<TDocument>> builderAction);

        /// <summary>
        /// Asynchronously retrieves multiple documents using a configured query builder.
        /// </summary>
        /// <param name="builder">Configured query builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns a collection of documents.</returns>
        /// <exception cref="DocumentException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<IEnumerable<TDocument>> GetRangeAsync(RangeQueryDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves multiple documents by configuring the query builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the query builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns a collection of documents.</returns>
        /// <exception cref="DocumentException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<IEnumerable<TDocument>> GetRangeAsync(Action<RangeQueryDocumentBuilder<TDocument>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves multiple documents using a configured query builder.
        /// </summary>
        /// <param name="builder">Configured query builder</param>
        /// <returns>A collection of documents.</returns>
        /// <exception cref="DocumentException">Thrown when database operation fails</exception>
        public IEnumerable<TDocument> GetRange(RangeQueryDocumentBuilder<TDocument> builder);

        /// <summary>
        /// Retrieves multiple documents by configuring the query builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the query builder</param>
        /// <returns>A collection of documents.</returns>
        /// <exception cref="DocumentException">Thrown when database operation fails</exception>
        public IEnumerable<TDocument> GetRange(Action<RangeQueryDocumentBuilder<TDocument>> builderAction);

        /// <summary>
        /// Asynchronously retrieves a range of documents with total count.
        /// </summary>
        /// <param name="builder">Configured query builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A <see cref="DocumentChunkDetails{TDocument}"/> chunk of documents with total count.</returns>
        /// <exception cref="DocumentException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<DocumentChunkDetails<TDocument>> GetRangeEntireAsync(RangeQueryDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves a range of documents with total count by configuring the query builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the query builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A <see cref="DocumentChunkDetails{TDocument}"/> chunk of documents with total count.</returns>
        /// <exception cref="DocumentException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<DocumentChunkDetails<TDocument>> GetRangeEntireAsync(Action<RangeQueryDocumentBuilder<TDocument>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a range of documents with total count.
        /// </summary>
        /// <param name="builder">Configured query builder</param>
        /// <returns>A <see cref="DocumentChunkDetails{TDocument}"/> chunk of documents with total count.</returns>
        /// <exception cref="DocumentException">Thrown when database operation fails</exception>
        public DocumentChunkDetails<TDocument> GetRangeEntire(RangeQueryDocumentBuilder<TDocument> builder);

        /// <summary>
        /// Retrieves a range of documents with total count by configuring the query builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the query builder</param>
        /// <returns>A <see cref="DocumentChunkDetails{TDocument}"/> chunk of documents with total count.</returns>
        /// <exception cref="DocumentException">Thrown when database operation fails</exception>
        public DocumentChunkDetails<TDocument> GetRangeEntire(Action<RangeQueryDocumentBuilder<TDocument>> builderAction);
    }
}
