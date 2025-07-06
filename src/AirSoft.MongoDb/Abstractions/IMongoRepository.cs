using AirSoft.MongoDb.Builders.Query;
using AirSoft.MongoDb.Builders.State.Insert;
using AirSoft.MongoDb.Builders.State.Remove;
using AirSoft.MongoDb.Builders.State.Replace;
using AirSoft.MongoDb.Details;
using AirSoft.MongoDb.Documents;
using AirSoft.MongoDb.MongoContexts;
using MongoDB.Bson;
using System.Linq.Expressions;

namespace AirSoft.MongoDb.Abstractions
{
    /// <summary>
    /// Represents a MongoDB-specific repository pattern for performing CRUD operations on documents of type <typeparamref name="TDocument"/>.
    /// Provides document operations with support for transactions, timeouts and cancellation.
    /// </summary>
    /// <typeparam name="TDocument">The type of document the repository will handle, which must inherit from <see cref="DocumentBase"/>.</typeparam>
    public interface IMongoRepository<TDocument> where TDocument : DocumentBase
    {
        /// <summary>
        /// Asynchronously checks existing record that match the specified filter.
        /// </summary>
        /// <param name="filter">The filter expression to apply to set find filter.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>The true if record is exits otherwise false.</returns>
        public Task<bool> IsExistsAsync(Expression<Func<TDocument, bool>> filter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves the count of documents that match the specified filter.
        /// </summary>
        /// <param name="filter">Optional filter expression</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The number of matching documents</returns>
        Task<long> GetCountAsync(Expression<Func<TDocument, bool>>? filter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves a document by its identifier.
        /// </summary>
        /// <param name="id">Document identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The document if found, otherwise null</returns>
        Task<TDocument?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves a single document using a configured query builder.
        /// </summary>
        /// <param name="builder">Configured query builder with filter, sorting and timeout options</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The document if found, otherwise null</returns>
        Task<TDocument?> GetSingleAsync(SingleQueryDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves multiple documents using a configured query builder.
        /// </summary>
        /// <param name="builder">Configured query builder with filter, sorting, pagination and timeout options</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of matching documents</returns>
        Task<IEnumerable<TDocument>> GetRangeAsync(RangeQueryDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves a range of documents with total count of matching documents.
        /// </summary>
        /// <param name="builder">Configured query builder with filter, sorting, pagination and timeout options</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A <see cref="ChunkDetails{TDocument}"/> containing the document subset and total count</returns>
        Task<DocumentChunkDetails<TDocument>> GetRangeEntireAsync(RangeQueryDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously inserts a single document using a configured builder.
        /// </summary>
        /// <param name="builder">Configured insert builder with document, options and transaction support</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task InsertAsync(InsertSingleDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously inserts multiple documents using a configured builder.
        /// </summary>
        /// <param name="builder">Configured insert builder with documents, options and transaction support</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task InsertRangeAsync(InsertRangeDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously removes a single document using a configured builder.
        /// </summary>
        /// <param name="builder">Configured remove builder with document identifier, options and transaction support</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task RemoveAsync(RemoveSingleDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously removes multiple documents using a configured builder.
        /// </summary>
        /// <param name="builder">Configured remove builder with document identifiers, options and transaction support</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task RemoveRangeAsync(RemoveRangeDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously replaces a single document using a configured builder.
        /// </summary>
        /// <param name="builder">Configured replace builder with document, options and transaction support</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task ReplaceAsync(ReplaceSingleDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously replaces multiple documents using a configured builder.
        /// </summary>
        /// <param name="builder">Configured replace builder with documents, options and transaction support</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task ReplaceRangeAsync(ReplaceRangeDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Represents a repository for interacting with MongoDB for a specific document type and a specific context.
    /// </summary>
    /// <typeparam name="TMongoContext">The type of the MongoDB context, which must inherit from <see cref="MongoContext"/>.</typeparam>
    /// <typeparam name="TDocument">The type of document the repository will handle, which must inherit from <see cref="DocumentBase"/>.</typeparam>
    public interface IMongoRepository<TDocument, TMongoContext> :
        IMongoRepository<TDocument>
        where TDocument : DocumentBase
        where TMongoContext : MongoContext;
}
