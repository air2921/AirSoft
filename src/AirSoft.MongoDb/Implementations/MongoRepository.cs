using AirSoft.Exceptions;
using AirSoft.MongoDb.Abstractions;
using AirSoft.MongoDb.Builders.Query;
using AirSoft.MongoDb.Builders.State.Insert;
using AirSoft.MongoDb.Builders.State.Remove;
using AirSoft.MongoDb.Builders.State.Replace;
using AirSoft.MongoDb.Details;
using AirSoft.MongoDb.Documents;
using AirSoft.MongoDb.Enums;
using AirSoft.MongoDb.Extensions;
using AirSoft.MongoDb.MongoContexts;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace AirSoft.MongoDb.Implementations
{
    /// <summary>
    /// A generic repository class for performing CRUD operations on a MongoDB collection.
    /// This class supports working with a specific MongoDB context and document type.
    /// </summary>
    /// <typeparam name="TMongoContext">The type of the MongoDB context, which must inherit from <see cref="MongoContext"/>.</typeparam>
    /// <typeparam name="TDocument">The type of the document, which must inherit from <see cref="DocumentBase"/>.</typeparam>
    /// <remarks>
    /// This class provides methods for querying, inserting, updating, and deleting documents in a MongoDB collection.
    /// It uses the provided <typeparamref name="TMongoContext"/> to access the database and ensures thread-safe operations.
    /// </remarks>
    public sealed class MongoRepository<TDocument, TMongoContext>(TMongoContext context, TDocument currentDocument)
        : IMongoRepository<TDocument>, IMongoRepository<TDocument, TMongoContext>
        where TDocument : DocumentBase, new()
        where TMongoContext : MongoContext
    {
        private readonly Lazy<IMongoCollection<TDocument>> _collection = new(() => context.SetDocument(currentDocument), LazyThreadSafetyMode.ExecutionAndPublication);

        #region Immutable

        /// <summary>Timeout for IsExists operations (10 seconds).</summary>
        private const int CheckExistTimeout = 10;

        /// <summary>Timeout for GetCount operations (20 seconds).</summary>
        private const int GetCountTimeout = 20;

        /// <summary>Timeout for GetRange operations (20 seconds).</summary>
        private const int GetRangeTimeout = 20;

        /// <summary>Timeout for GetSingle operations (20 seconds).</summary>
        private const int GetSingleTimeout = 20;

        /// <summary>Timeout for GetById operations (20 seconds).</summary>
        private const int GetByIdTimeout = 20;

        /// <summary>Timeout for Insert operations (20 seconds).</summary>
        private const int InsertTimeout = 20;

        /// <summary>Timeout for InsertRange operations (20 seconds).</summary>
        private const int InsertRangeTimeout = 20;

        /// <summary>Timeout for RemoveRange operations (40 seconds).</summary>
        private const int RemoveRangeTimeout = 40;

        /// <summary>Timeout for Remove operations (20 seconds).</summary>
        private const int RemoveTimeout = 20;

        /// <summary>Timeout for Replace operations (20 seconds).</summary>
        private const int ReplaceTimeout = 20;

        /// <summary>Timeout for ReplaceRange operations (40 seconds).</summary>
        private const int ReplaceRangeTimeout = 40;

        #endregion

        /// <summary>
        /// Asynchronously checks existing record that match the specified filter.
        /// </summary>
        /// <param name="filter">The filter expression to apply to set find filter.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>The true if record is exits otherwise false.</returns>
        /// <exception cref="DocumentException">Thrown when an error occurs during the count operation.</exception>
        public async Task<bool> IsExistsAsync(Expression<Func<TDocument, bool>> filter, CancellationToken cancellationToken = default)
        {
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(CheckExistTimeout));
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                return await _collection.Value.Find(filter).AnyAsync(cancellationToken);
            }
            catch (OperationCanceledException ex)
            {
                throw new DocumentException("The operation was cancelled due to waiting too long for completion or due to manual cancellation", ex);
            }
            catch (Exception ex)
            {
                throw new DocumentException("An error occurred while attempting to retrieve count of documents", ex);
            }
        }

        /// <summary>
        /// Asynchronously retrieves the count of documents matching the specified query.
        /// </summary>
        /// <param name="filter">An optional filter expression to count matching documents.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>The number of documents matching the query.</returns>
        /// <exception cref="DocumentException">Thrown when an error occurs during the count operation.</exception>
        public async Task<long> GetCountAsync(Expression<Func<TDocument, bool>>? filter, CancellationToken cancellationToken = default)
        {
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(GetCountTimeout));
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                return await _collection.Value.Find(filter ?? Builders<TDocument>.Filter.Empty).CountDocumentsAsync(cancellationToken);
            }
            catch (OperationCanceledException ex)
            {
                throw new DocumentException("The operation was cancelled due to waiting too long for completion or due to manual cancellation", ex);
            }
            catch (Exception ex)
            {
                throw new DocumentException("An error occurred while attempting to retrieve count of documents", ex);
            }
        }

        /// <summary>
        /// Asynchronously retrieves a document by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the document to retrieve.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>The document if found, otherwise null.</returns>
        /// <exception cref="DocumentException">Thrown when an error occurs during the retrieval operation.</exception>
        public async Task<TDocument?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
        {
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(GetByIdTimeout));
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                var filter = Builders<TDocument>.Filter.Eq(x => x.Id, id);
                return await _collection.Value.Find(filter).FirstOrDefaultAsync(cancellationToken);
            }
            catch (OperationCanceledException ex)
            {
                throw new DocumentException("The operation was cancelled due to waiting too long for completion or due to manual cancellation", ex);
            }
            catch (Exception ex)
            {
                throw new DocumentException("An error occurred while attempting to retrieve document", ex);
            }
        }

        /// <summary>
        /// Asynchronously retrieves a single document based on the specified query builder.
        /// </summary>
        /// <param name="builder">A <see cref="SingleQueryDocumentBuilder{TDocument}"/> that defines the query criteria.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>The document if found matching the criteria, otherwise null.</returns>
        /// <exception cref="DocumentException">Thrown when an error occurs during the retrieval operation.</exception>
        public async Task<TDocument?> GetSingleAsync(SingleQueryDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default)
        {
            try
            {
                var timeout = builder.Timeout == TimeSpan.Zero ? TimeSpan.FromSeconds(GetSingleTimeout) : builder.Timeout;
                using var cts = new CancellationTokenSource(timeout);
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                var query = _collection.Value.ApplyBuilder(builder);
                return await query.FirstOrDefaultAsync(cancellationToken);
            }
            catch (OperationCanceledException ex)
            {
                throw new DocumentException("The operation was cancelled due to waiting too long for completion or due to manual cancellation", ex);
            }
            catch (Exception ex)
            {
                throw new DocumentException("An error occurred while attempting to retrieve document", ex);
            }
        }

        /// <summary>
        /// Asynchronously retrieves multiple documents based on the specified query builder.
        /// </summary>
        /// <param name="builder">A <see cref="RangeQueryDocumentBuilder{TDocument}"/> that defines the query criteria.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>A collection of documents matching the query criteria.</returns>
        /// <exception cref="DocumentException">Thrown when an error occurs during the retrieval operation.</exception>
        public async Task<IEnumerable<TDocument>> GetRangeAsync(RangeQueryDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default)
        {
            try
            {
                var timeout = builder.Timeout == TimeSpan.Zero ? TimeSpan.FromSeconds(GetRangeTimeout) : builder.Timeout;
                using var cts = new CancellationTokenSource(timeout);
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                var query = _collection.Value.ApplyBuilder(builder);
                return await query.ToListAsync(cancellationToken);
            }
            catch (OperationCanceledException ex)
            {
                throw new DocumentException("The operation was cancelled due to waiting too long for completion or due to manual cancellation", ex);
            }
            catch (Exception ex)
            {
                throw new DocumentException("An error occurred while attempting to retrieve a range of documents", ex);
            }
        }

        /// <summary>
        /// Asynchronously retrieves a paginated set of documents along with the total count of matching documents.
        /// </summary>
        /// <param name="builder">A <see cref="RangeQueryDocumentBuilder{TDocument}"/> that defines the query criteria.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>A <see cref="ChunkDetails{TDocument}"/> containing the paginated results and total count.</returns>
        /// <exception cref="DocumentException">Thrown when an error occurs during the retrieval operation.</exception>
        public async Task<DocumentChunkDetails<TDocument>> GetRangeEntireAsync(RangeQueryDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default)
        {
            try
            {
                var timeout = builder.Timeout == TimeSpan.Zero ? TimeSpan.FromSeconds(GetRangeTimeout) : builder.Timeout;
                using var cts = new CancellationTokenSource(timeout);
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                var collectionQuery = _collection.Value.ApplyBuilder(builder);
                var countQuery = _collection.Value.Find(builder.Filter ?? Builders<TDocument>.Filter.Empty);

                var chunk = await collectionQuery.ToListAsync(cancellationToken);
                var count = await countQuery.CountDocumentsAsync(cancellationToken);

                return new DocumentChunkDetails<TDocument>
                {
                    Chunk = chunk,
                    Total = count,
                };
            }
            catch (OperationCanceledException ex)
            {
                throw new DocumentException("The operation was cancelled due to waiting too long for completion or due to manual cancellation", ex);
            }
            catch (Exception ex)
            {
                throw new DocumentException("An error occurred while attempting to retrieve a range of documents", ex);
            }
        }

        /// <summary>
        /// Asynchronously inserts a single document using the specified builder.
        /// </summary>
        /// <param name="builder">A <see cref="InsertSingleDocumentBuilder{TDocument}"/> containing the document to insert and options.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <exception cref="DocumentException">Thrown when an error occurs during the insert operation.</exception>
        public async Task InsertAsync(InsertSingleDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default)
        {
            try
            {
                var timeout = builder.Timeout == TimeSpan.Zero ? TimeSpan.FromSeconds(InsertTimeout) : builder.Timeout;
                using var cts = new CancellationTokenSource(timeout);
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                if (builder.Session is null)
                    await _collection.Value.InsertOneAsync(builder.Document, builder.Options, cancellationToken);
                else
                    await _collection.Value.InsertOneAsync(builder.Session, builder.Document, builder.Options, cancellationToken);
            }
            catch (OperationCanceledException ex)
            {
                throw new DocumentException("The operation was cancelled due to waiting too long for completion or due to manual cancellation", ex);
            }
            catch (Exception ex)
            {
                throw new DocumentException("An error occurred while attempting to insert document", ex);
            }
        }

        /// <summary>
        /// Asynchronously inserts multiple documents using the specified builder.
        /// </summary>
        /// <param name="builder">A <see cref="InsertRangeDocumentBuilder{TDocument}"/> containing the documents to insert and options.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <exception cref="DocumentException">Thrown when an error occurs during the insert operation.</exception>
        public async Task InsertRangeAsync(InsertRangeDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default)
        {
            try
            {
                var timeout = builder.Timeout == TimeSpan.Zero ? TimeSpan.FromSeconds(InsertRangeTimeout) : builder.Timeout;
                using var cts = new CancellationTokenSource(timeout);
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                if (builder.Session is null)
                    await _collection.Value.InsertManyAsync(builder.Documents, builder.Options, cancellationToken);
                else
                    await _collection.Value.InsertManyAsync(builder.Session, builder.Documents, builder.Options, cancellationToken);
            }
            catch (OperationCanceledException ex)
            {
                throw new DocumentException("The operation was cancelled due to waiting too long for completion or due to manual cancellation", ex);
            }
            catch (Exception ex)
            {
                throw new DocumentException("An error occurred while attempting to insert documents", ex);
            }
        }

        /// <summary>
        /// Asynchronously removes a single document using the specified builder.
        /// </summary>
        /// <param name="builder">A <see cref="RemoveSingleDocumentBuilder{TDocument}"/> containing the document identifier and options.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <exception cref="DocumentException">Thrown when an error occurs during the remove operation.</exception>
        public async Task RemoveAsync(RemoveSingleDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default)
        {
            try
            {
                var timeout = builder.Timeout == TimeSpan.Zero ? TimeSpan.FromSeconds(RemoveTimeout) : builder.Timeout;
                using var cts = new CancellationTokenSource(timeout);
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                var filter = DefineSingleRemoveFilter(builder);

                if (builder.Session is null)
                    await _collection.Value.DeleteOneAsync(filter, builder.Options, cancellationToken);
                else
                    await _collection.Value.DeleteOneAsync(builder.Session, filter, builder.Options, cancellationToken);
            }
            catch (OperationCanceledException ex)
            {
                throw new DocumentException("The operation was cancelled due to waiting too long for completion or due to manual cancellation", ex);
            }
            catch (Exception ex) when (ex is not DocumentException)
            {
                throw new DocumentException("An error occurred while attempting to remove document", ex);
            }
        }

        /// <summary>
        /// Asynchronously removes multiple documents using the specified builder.
        /// </summary>
        /// <param name="builder">A <see cref="RemoveRangeDocumentBuilder{TDocument}"/> containing the document identifiers and options.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <exception cref="DocumentException">Thrown when an error occurs during the remove operation.</exception>
        public async Task RemoveRangeAsync(RemoveRangeDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default)
        {
            try
            {
                var timeout = builder.Timeout == TimeSpan.Zero ? TimeSpan.FromSeconds(RemoveRangeTimeout) : builder.Timeout;
                using var cts = new CancellationTokenSource(timeout);
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                var filter = DefineRangeRemoveFilter(builder);

                if (builder.Session is null)
                    await _collection.Value.DeleteManyAsync(filter, builder.Options, cancellationToken);
                else
                    await _collection.Value.DeleteManyAsync(builder.Session, filter, builder.Options, cancellationToken);
            }
            catch (OperationCanceledException ex)
            {
                throw new DocumentException("The operation was cancelled due to waiting too long for completion or due to manual cancellation", ex);
            }
            catch (Exception ex) when (ex is not DocumentException)
            {
                throw new DocumentException("An error occurred while attempting to remove range of documents", ex);
            }
        }

        /// <summary>
        /// Asynchronously replaces a single document using the specified builder.
        /// </summary>
        /// <param name="builder">A <see cref="ReplaceSingleDocumentBuilder{TDocument}"/> containing the document to replace and options.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <exception cref="DocumentException">Thrown when an error occurs during the replace operation.</exception>
        public async Task ReplaceAsync(ReplaceSingleDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default)
        {
            try
            {
                var timeout = builder.Timeout == TimeSpan.Zero ? TimeSpan.FromSeconds(ReplaceTimeout) : builder.Timeout;
                using var cts = new CancellationTokenSource(timeout);
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                var filter = Builders<TDocument>.Filter.Eq(x => x.Id, builder.Document.Id);

                builder.Document.UpdatedAt = DateTimeOffset.UtcNow;

                if (builder.Session is null)
                    await _collection.Value.ReplaceOneAsync(filter, builder.Document, builder.Options, cancellationToken);
                else
                    await _collection.Value.ReplaceOneAsync(builder.Session, filter, builder.Document, builder.Options, cancellationToken);
            }
            catch (OperationCanceledException ex)
            {
                throw new DocumentException("The operation was cancelled due to waiting too long for completion or due to manual cancellation", ex);
            }
            catch (Exception ex)
            {
                throw new DocumentException("An error occurred while attempting to replace document", ex);
            }
        }

        /// <summary>
        /// Asynchronously replaces multiple documents using the specified builder.
        /// </summary>
        /// <param name="builder">A <see cref="ReplaceRangeDocumentBuilder{TDocument}"/> containing the documents to replace and options.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <exception cref="DocumentException">Thrown when an error occurs during the replace operation.</exception>
        public async Task ReplaceRangeAsync(ReplaceRangeDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default)
        {
            try
            {
                var timeout = builder.Timeout == TimeSpan.Zero ? TimeSpan.FromSeconds(ReplaceRangeTimeout) : builder.Timeout;
                using var cts = new CancellationTokenSource(timeout);
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                var bulk = builder.Documents.Select(doc =>
                {
                    doc.UpdatedAt = DateTimeOffset.UtcNow;
                    var filter = Builders<TDocument>.Filter.Eq(x => x.Id, doc.Id);
                    return new ReplaceOneModel<TDocument>(filter, doc);
                });

                if (bulk.Any())
                {
                    var options = new BulkWriteOptions { IsOrdered = false };

                    if (builder.Session is null)
                        await _collection.Value.BulkWriteAsync(bulk, options, cancellationToken);
                    else
                        await _collection.Value.BulkWriteAsync(builder.Session, bulk, options, cancellationToken);
                }
            }
            catch (OperationCanceledException ex)
            {
                throw new DocumentException("The operation was cancelled due to waiting too long for completion or due to manual cancellation", ex);
            }
            catch (Exception ex)
            {
                throw new DocumentException("An error occurred while attempting to replace range of documents", ex);
            }
        }

        /// <summary>
        /// Defines a filter for removing a single document based on the provided builder and remove mode.
        /// </summary>
        /// <param name="builder">The <see cref="RemoveSingleDocumentBuilder{TDocument}"/> containing removal criteria.</param>
        /// <param name="document">The document instance used for type conversion reference.</param>
        /// <returns>A <see cref="FilterDefinition{TDocument}"/> configured for the removal operation.</returns>
        /// <exception cref="DocumentException">Throws when invalid RemoveMode</exception>
        private static FilterDefinition<TDocument> DefineSingleRemoveFilter(RemoveSingleDocumentBuilder<TDocument> builder)
        {
            if (builder.RemoveMode == DocumentRemoveMode.Filter && builder.Filter is not null)
                return Builders<TDocument>.Filter.Where(builder.Filter);
            if (builder.RemoveMode == DocumentRemoveMode.Identifier && builder.Id is not null)
                return Builders<TDocument>.Filter.Eq(x => x.Id, builder.Id);
            if (builder.RemoveMode == DocumentRemoveMode.Document && builder.Document is not null)
                return Builders<TDocument>.Filter.Eq(x => x.Id, builder.Document.Id);

            throw new DocumentException($"Invalid {nameof(builder.RemoveMode)}");
        }

        /// <summary>
        /// Defines a filter for removing multiple documents based on the provided builder and remove mode.
        /// </summary>
        /// <param name="builder">The <see cref="RemoveRangeDocumentBuilder{TDocument}"/> containing removal criteria.</param>
        /// <param name="document">The document instance used for type conversion reference.</param>
        /// <returns>A <see cref="FilterDefinition{TDocument}"/> configured for the bulk removal operation.</returns>
        /// <exception cref="DocumentException">Throws when invalid RemoveMode</exception>
        private static FilterDefinition<TDocument> DefineRangeRemoveFilter(RemoveRangeDocumentBuilder<TDocument> builder)
        {
            if (builder.RemoveMode == DocumentRemoveMode.Filter && builder.Filter is not null)
                return Builders<TDocument>.Filter.Where(builder.Filter);
            if (builder.RemoveMode == DocumentRemoveMode.Identifier)
                return Builders<TDocument>.Filter.In(x => x.Id, builder.Identifiers);
            if (builder.RemoveMode == DocumentRemoveMode.Document)
                return Builders<TDocument>.Filter.In(x => x.Id, builder.Documents.Select(doc => doc.Id));

            throw new DocumentException($"Invalid {nameof(builder.RemoveMode)}");
        }
    }
}
