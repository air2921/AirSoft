using AirSoft.Exceptions;
using AirSoft.MongoDb.Abstractions;
using AirSoft.MongoDb.Abstractions.Builders.Query;
using AirSoft.MongoDb.Abstractions.Builders.State.Insert;
using AirSoft.MongoDb.Abstractions.Builders.State.Remove;
using AirSoft.MongoDb.Abstractions.Builders.State.Replace;
using AirSoft.MongoDb.Abstractions.Details;
using AirSoft.MongoDb.Abstractions.Documents;
using AirSoft.MongoDb.Abstractions.Enums;
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
    public partial class MongoRepository<TDocument, TMongoContext>(TMongoContext context, TDocument currentDocument)
        : IMongoRepository<TDocument>
        where TDocument : DocumentBase, new()
        where TMongoContext : MongoContext
    {
        private readonly Lazy<IMongoCollection<TDocument>> _collection = new(() => context.SetDocument(currentDocument), LazyThreadSafetyMode.ExecutionAndPublication);

        #region Immutable

        /// <summary>Timeout for IsExists operations (10 seconds).</summary>
        private const int CheckExistTimeout = 10;

        /// <summary>Timeout for GetCount operations (10 seconds).</summary>
        private const int GetCountTimeout = 10;

        /// <summary>Timeout for GetRange operations (10 seconds).</summary>
        private const int GetRangeTimeout = 10;

        /// <summary>Timeout for GetSingle operations (10 seconds).</summary>
        private const int GetSingleTimeout = 10;

        /// <summary>Timeout for GetById operations (10 seconds).</summary>
        private const int GetByIdTimeout = 10;

        /// <summary>Timeout for Insert operations (10 seconds).</summary>
        private const int InsertTimeout = 10;

        /// <summary>Timeout for InsertRange operations (20 seconds).</summary>
        private const int InsertRangeTimeout = 20;

        /// <summary>Timeout for RemoveRange operations (20 seconds).</summary>
        private const int RemoveRangeTimeout = 20;

        /// <summary>Timeout for Remove operations (10 seconds).</summary>
        private const int RemoveTimeout = 10;

        /// <summary>Timeout for Replace operations (10 seconds).</summary>
        private const int ReplaceTimeout = 10;

        /// <summary>Timeout for ReplaceRange operations (20 seconds).</summary>
        private const int ReplaceRangeTimeout = 20;

        #endregion

        /// <summary>
        /// Defines a filter for removing a single document based on the provided builder and remove mode.
        /// </summary>
        /// <param name="builder">The <see cref="RemoveSingleDocumentBuilder{TDocument}"/> containing removal criteria.</param>
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

    public partial class MongoRepository<TDocument, TMongoContext>
    {
        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public bool IsExists(Expression<Func<TDocument, bool>> filter)
        {
            try
            {
                return _collection.Value.Find(filter).Any();
            }
            catch (Exception ex)
            {
                throw new DocumentException("An error occurred while attempting to retrieve count of documents", ex);
            }
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public long GetCount(Expression<Func<TDocument, bool>>? filter)
        {
            try
            {
                return _collection.Value.Find(filter ?? Builders<TDocument>.Filter.Empty).CountDocuments();
            }
            catch (Exception ex)
            {
                throw new DocumentException("An error occurred while attempting to retrieve count of documents", ex);
            }
        }
    }

    public partial class MongoRepository<TDocument, TMongoContext>
    {
        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public TDocument? GetById(ObjectId id)
        {
            try
            {
                var filter = Builders<TDocument>.Filter.Eq(x => x.Id, id);
                return _collection.Value.Find(filter).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new DocumentException("An error occurred while attempting to retrieve document", ex);
            }
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public async Task<TDocument?> GetSingleAsync(Action<SingleQueryDocumentBuilder<TDocument>> builderAction, CancellationToken cancellationToken = default)
        {
            var builder = new SingleQueryDocumentBuilder<TDocument>();
            builderAction(builder);
            return await GetSingleAsync(builder, cancellationToken);
        }

        /// <inheritdoc/>
        public TDocument? GetSingle(SingleQueryDocumentBuilder<TDocument> builder)
        {
            try
            {
                var query = _collection.Value.ApplyBuilder(builder);
                return query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new DocumentException("An error occurred while attempting to retrieve document", ex);
            }
        }

        /// <inheritdoc/>
        public TDocument? GetSingle(Action<SingleQueryDocumentBuilder<TDocument>> builderAction)
        {
            var builder = new SingleQueryDocumentBuilder<TDocument>();
            builderAction(builder);
            return GetSingle(builder);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public async Task<IEnumerable<TDocument>> GetRangeAsync(Action<RangeQueryDocumentBuilder<TDocument>> builderAction, CancellationToken cancellationToken = default)
        {
            var builder = new RangeQueryDocumentBuilder<TDocument>();
            builderAction(builder);
            return await GetRangeAsync(builder, cancellationToken);
        }

        /// <inheritdoc/>
        public IEnumerable<TDocument> GetRange(RangeQueryDocumentBuilder<TDocument> builder)
        {
            try
            {
                var query = _collection.Value.ApplyBuilder(builder);
                return query.ToList();
            }
            catch (Exception ex)
            {
                throw new DocumentException("An error occurred while attempting to retrieve a range of documents", ex);
            }
        }

        /// <inheritdoc/>
        public IEnumerable<TDocument> GetRange(Action<RangeQueryDocumentBuilder<TDocument>> builderAction)
        {
            var builder = new RangeQueryDocumentBuilder<TDocument>();
            builderAction(builder);
            return GetRange(builder);
        }

        /// <inheritdoc/>
        public async Task<DocumentChunkDetails<TDocument>> GetRangeEntireAsync(RangeQueryDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default)
        {
            try
            {
                var timeout = builder.Timeout == TimeSpan.Zero ? TimeSpan.FromSeconds(GetRangeTimeout) : builder.Timeout;
                using var cts = new CancellationTokenSource(timeout);
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                var collectionQuery = _collection.Value.ApplyBuilder(builder);

                var filters = builder.Filters.Select(expr => Builders<TDocument>.Filter.Where(expr)).ToArray();
                var filter = filters.Length != 0 ? Builders<TDocument>.Filter.And(filters) : Builders<TDocument>.Filter.Empty;

                var countQuery = _collection.Value.Find(filter);

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

        /// <inheritdoc/>
        public async Task<DocumentChunkDetails<TDocument>> GetRangeEntireAsync(Action<RangeQueryDocumentBuilder<TDocument>> builderAction, CancellationToken cancellationToken = default)
        {
            var builder = new RangeQueryDocumentBuilder<TDocument>();
            builderAction(builder);
            return await GetRangeEntireAsync(builder, cancellationToken);
        }

        /// <inheritdoc/>
        public DocumentChunkDetails<TDocument> GetRangeEntire(RangeQueryDocumentBuilder<TDocument> builder)
        {
            try
            {
                var collectionQuery = _collection.Value.ApplyBuilder(builder);

                var filters = builder.Filters.Select(expr => Builders<TDocument>.Filter.Where(expr)).ToArray();
                var filter = filters.Length != 0 ? Builders<TDocument>.Filter.And(filters) : Builders<TDocument>.Filter.Empty;

                var countQuery = _collection.Value.Find(filter);

                var chunk = collectionQuery.ToList();
                var count = countQuery.CountDocuments();

                return new DocumentChunkDetails<TDocument>
                {
                    Chunk = chunk,
                    Total = count,
                };
            }
            catch (Exception ex)
            {
                throw new DocumentException("An error occurred while attempting to retrieve a range of documents", ex);
            }
        }

        /// <inheritdoc/>
        public DocumentChunkDetails<TDocument> GetRangeEntire(Action<RangeQueryDocumentBuilder<TDocument>> builderAction)
        {
            var builder = new RangeQueryDocumentBuilder<TDocument>();
            builderAction(builder);
            return GetRangeEntire(builder);
        }
    }

    public partial class MongoRepository<TDocument, TMongoContext>
    {
        /// <inheritdoc/>
        public async Task<long> InsertAsync(InsertSingleDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default)
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

                return 1;
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

        /// <inheritdoc/>
        public async Task<long> InsertAsync(Action<InsertSingleDocumentBuilder<TDocument>> builderAction, CancellationToken cancellationToken = default)
        {
            var builder = new InsertSingleDocumentBuilder<TDocument>();
            builderAction(builder);
            return await InsertAsync(builder, cancellationToken);
        }

        /// <inheritdoc/>
        public long Insert(InsertSingleDocumentBuilder<TDocument> builder)
        {
            try
            {
                if (builder.Session is null)
                    _collection.Value.InsertOne(builder.Document, builder.Options);
                else
                    _collection.Value.InsertOne(builder.Session, builder.Document, builder.Options);

                return 1;
            }
            catch (Exception ex)
            {
                throw new DocumentException("An error occurred while attempting to insert document", ex);
            }
        }

        /// <inheritdoc/>
        public long Insert(Action<InsertSingleDocumentBuilder<TDocument>> builderAction)
        {
            var builder = new InsertSingleDocumentBuilder<TDocument>();
            builderAction(builder);
            return Insert(builder);
        }

        /// <inheritdoc/>
        public async Task<long> InsertRangeAsync(InsertRangeDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default)
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

                return builder.Documents.Count;
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

        /// <inheritdoc/>
        public async Task<long> InsertRangeAsync(Action<InsertRangeDocumentBuilder<TDocument>> builderAction, CancellationToken cancellationToken = default)
        {
            var builder = new InsertRangeDocumentBuilder<TDocument>();
            builderAction(builder);
            return await InsertRangeAsync(builder, cancellationToken);
        }

        /// <inheritdoc/>
        public long InsertRange(InsertRangeDocumentBuilder<TDocument> builder)
        {
            try
            {
                if (builder.Session is null)
                    _collection.Value.InsertMany(builder.Documents, builder.Options);
                else
                    _collection.Value.InsertMany(builder.Session, builder.Documents, builder.Options);

                return builder.Documents.Count;
            }
            catch (Exception ex)
            {
                throw new DocumentException("An error occurred while attempting to insert documents", ex);
            }
        }

        /// <inheritdoc/>
        public long InsertRange(Action<InsertRangeDocumentBuilder<TDocument>> builderAction)
        {
            var builder = new InsertRangeDocumentBuilder<TDocument>();
            builderAction(builder);
            return InsertRange(builder);
        }
    }

    public partial class MongoRepository<TDocument, TMongoContext>
    {
        /// <inheritdoc/>
        public async Task<long> RemoveAsync(RemoveSingleDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default)
        {
            try
            {
                var timeout = builder.Timeout == TimeSpan.Zero ? TimeSpan.FromSeconds(RemoveTimeout) : builder.Timeout;
                using var cts = new CancellationTokenSource(timeout);
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                var filter = DefineSingleRemoveFilter(builder);
                DeleteResult result = null!;

                if (builder.Session is null)
                    result = await _collection.Value.DeleteOneAsync(filter, builder.Options, cancellationToken);
                else
                    result = await _collection.Value.DeleteOneAsync(builder.Session, filter, builder.Options, cancellationToken);

                return result.DeletedCount;
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

        /// <inheritdoc/>
        public async Task<long> RemoveAsync(Action<RemoveSingleDocumentBuilder<TDocument>> builderAction, CancellationToken cancellationToken = default)
        {
            var builder = new RemoveSingleDocumentBuilder<TDocument>();
            builderAction(builder);
            return await RemoveAsync(builder, cancellationToken);
        }

        /// <inheritdoc/>
        public long Remove(RemoveSingleDocumentBuilder<TDocument> builder)
        {
            try
            {
                var filter = DefineSingleRemoveFilter(builder);
                DeleteResult result = null!;

                if (builder.Session is null)
                    result = _collection.Value.DeleteOne(filter, builder.Options);
                else
                    result = _collection.Value.DeleteOne(builder.Session, filter, builder.Options);

                return result.DeletedCount;
            }
            catch (Exception ex) when (ex is not DocumentException)
            {
                throw new DocumentException("An error occurred while attempting to remove document", ex);
            }
        }

        /// <inheritdoc/>
        public long Remove(Action<RemoveSingleDocumentBuilder<TDocument>> builderAction)
        {
            var builder = new RemoveSingleDocumentBuilder<TDocument>();
            builderAction(builder);
            return Remove(builder);
        }

        /// <inheritdoc/>
        public async Task<long> RemoveRangeAsync(RemoveRangeDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default)
        {
            try
            {
                var timeout = builder.Timeout == TimeSpan.Zero ? TimeSpan.FromSeconds(RemoveRangeTimeout) : builder.Timeout;
                using var cts = new CancellationTokenSource(timeout);
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                var filter = DefineRangeRemoveFilter(builder);
                DeleteResult result = null!;

                if (builder.Session is null)
                    result = await _collection.Value.DeleteManyAsync(filter, builder.Options, cancellationToken);
                else
                    result = await _collection.Value.DeleteManyAsync(builder.Session, filter, builder.Options, cancellationToken);

                return result.DeletedCount;
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

        /// <inheritdoc/>
        public async Task<long> RemoveRangeAsync(Action<RemoveRangeDocumentBuilder<TDocument>> builderAction, CancellationToken cancellationToken = default)
        {
            var builder = new RemoveRangeDocumentBuilder<TDocument>();
            builderAction(builder);
            return await RemoveRangeAsync(builder, cancellationToken);
        }

        /// <inheritdoc/>
        public long RemoveRange(RemoveRangeDocumentBuilder<TDocument> builder)
        {
            try
            {
                var filter = DefineRangeRemoveFilter(builder);
                DeleteResult result = null!;

                if (builder.Session is null)
                    result = _collection.Value.DeleteMany(filter, builder.Options);
                else
                    result = _collection.Value.DeleteMany(builder.Session, filter, builder.Options);

                return result.DeletedCount;
            }
            catch (Exception ex) when (ex is not DocumentException)
            {
                throw new DocumentException("An error occurred while attempting to remove range of documents", ex);
            }
        }

        /// <inheritdoc/>
        public long RemoveRange(Action<RemoveRangeDocumentBuilder<TDocument>> builderAction)
        {
            var builder = new RemoveRangeDocumentBuilder<TDocument>();
            builderAction(builder);
            return RemoveRange(builder);
        }
    }

    public partial class MongoRepository<TDocument, TMongoContext>
    {
        /// <inheritdoc/>
        public async Task<long> ReplaceAsync(ReplaceSingleDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default)
        {
            try
            {
                var timeout = builder.Timeout == TimeSpan.Zero ? TimeSpan.FromSeconds(ReplaceTimeout) : builder.Timeout;
                using var cts = new CancellationTokenSource(timeout);
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                var filter = Builders<TDocument>.Filter.Eq(x => x.Id, builder.Document.Id);

                builder.Document.UpdatedAt = DateTimeOffset.UtcNow;
                ReplaceOneResult result = null!;

                if (builder.Session is null)
                    result = await _collection.Value.ReplaceOneAsync(filter, builder.Document, builder.Options, cancellationToken);
                else
                    result = await _collection.Value.ReplaceOneAsync(builder.Session, filter, builder.Document, builder.Options, cancellationToken);

                return result.ModifiedCount;
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

        /// <inheritdoc/>
        public async Task<long> ReplaceAsync(Action<ReplaceSingleDocumentBuilder<TDocument>> builderAction, CancellationToken cancellationToken = default)
        {
            var builder = new ReplaceSingleDocumentBuilder<TDocument>();
            builderAction(builder);
            return await ReplaceAsync(builder, cancellationToken);
        }

        /// <inheritdoc/>
        public long Replace(ReplaceSingleDocumentBuilder<TDocument> builder)
        {
            try
            {
                var filter = Builders<TDocument>.Filter.Eq(x => x.Id, builder.Document.Id);

                builder.Document.UpdatedAt = DateTimeOffset.UtcNow;
                ReplaceOneResult result = null!;

                if (builder.Session is null)
                    result = _collection.Value.ReplaceOne(filter, builder.Document, builder.Options);
                else
                    result = _collection.Value.ReplaceOne(builder.Session, filter, builder.Document, builder.Options);

                return result.ModifiedCount;
            }
            catch (Exception ex)
            {
                throw new DocumentException("An error occurred while attempting to replace document", ex);
            }
        }

        /// <inheritdoc/>
        public long Replace(Action<ReplaceSingleDocumentBuilder<TDocument>> builderAction)
        {
            var builder = new ReplaceSingleDocumentBuilder<TDocument>();
            builderAction(builder);
            return Replace(builder);
        }

        /// <inheritdoc/>
        public async Task<long> ReplaceRangeAsync(ReplaceRangeDocumentBuilder<TDocument> builder, CancellationToken cancellationToken = default)
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

                return builder.Documents.Count;
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

        /// <inheritdoc/>
        public async Task<long> ReplaceRangeAsync(Action<ReplaceRangeDocumentBuilder<TDocument>> builderAction, CancellationToken cancellationToken = default)
        {
            var builder = new ReplaceRangeDocumentBuilder<TDocument>();
            builderAction(builder);
            return await ReplaceRangeAsync(builder, cancellationToken);
        }

        /// <inheritdoc/>
        public long ReplaceRange(ReplaceRangeDocumentBuilder<TDocument> builder)
        {
            try
            {
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
                        _collection.Value.BulkWrite(bulk, options);
                    else
                        _collection.Value.BulkWrite(builder.Session, bulk, options);
                }

                return builder.Documents.Count;
            }
            catch (Exception ex)
            {
                throw new DocumentException("An error occurred while attempting to replace range of documents", ex);
            }
        }

        /// <inheritdoc/>
        public long ReplaceRange(Action<ReplaceRangeDocumentBuilder<TDocument>> builderAction)
        {
            var builder = new ReplaceRangeDocumentBuilder<TDocument>();
            builderAction(builder);
            return ReplaceRange(builder);
        }
    }
}
