using AirSoft.EntityFrameworkCore.Abstractions;
using AirSoft.EntityFrameworkCore.Abstractions.Builders.Query;
using AirSoft.EntityFrameworkCore.Abstractions.Builders.State.Add;
using AirSoft.EntityFrameworkCore.Abstractions.Builders.State.Remove;
using AirSoft.EntityFrameworkCore.Abstractions.Builders.State.Restore;
using AirSoft.EntityFrameworkCore.Abstractions.Builders.State.Update;
using AirSoft.EntityFrameworkCore.Abstractions.Details;
using AirSoft.EntityFrameworkCore.Abstractions.Entities;
using AirSoft.EntityFrameworkCore.Abstractions.Enums;
using AirSoft.EntityFrameworkCore.Extensions;
using AirSoft.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AirSoft.EntityFrameworkCore.Implementations
{
    /// <summary>
    /// A generic repository class responsible for performing CRUD operations on entities in a database context.
    /// This class implements the <see cref="IRepository{TEntity}"/> interface.
    /// It utilizes Entity Framework Core to interact with a database and supports various query and manipulation methods.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity being managed by the repository. It must inherit from <see cref="EntityBase"/>.</typeparam>
    /// <typeparam name="TDbContext">The type of the database context. It must inherit from <see cref="DbContext"/>.</typeparam>
    /// <remarks>
    /// This repository class provides methods for querying, adding, updating, and deleting entities in a database.
    /// </remarks>
    public partial class Repository<TEntity, TDbContext> :
        IRepository<TEntity> where TEntity : EntityBase where TDbContext : DbContext
    {
        #region Fields and constructor

        private readonly TDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity, TDbContext}"/> class.
        /// </summary>
        /// <param name="context">An instance of the database context to interact with the underlying database.</param>
        public Repository(TDbContext context)
        {
            _context = context ?? throw new InvalidArgumentException("Database context cannot be null");
            _dbSet = _context.Set<TEntity>();
        }

        #endregion

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

        /// <summary>Timeout for Add operations (20 seconds).</summary>
        private const int AddTimeout = 20;

        /// <summary>Timeout for AddRange operations (20 seconds).</summary>
        private const int AddRangeTimeout = 20;

        /// <summary>Timeout for RemoveRange operations (40 seconds).</summary>
        private const int RemoveRangeTimeout = 40;

        /// <summary>Timeout for Remove operations (20 seconds).</summary>
        private const int RemoveTimeout = 20;

        /// <summary>Timeout for Update operations (20 seconds).</summary>
        private const int UpdateTimeout = 20;

        /// <summary>Timeout for UpdateRange operations (40 seconds).</summary>
        private const int UpdateRangeTimeout = 40;

        /// <summary>Timeout for Restore operations (20 seconds).</summary>
        private const int RestoreTimeout = 20;

        /// <summary>Timeout for RestoreRange operations (40 seconds).</summary>
        private const int RestoreRangeTimeout = 40;

        #endregion
    }

    public partial class Repository<TEntity, TDbContext>
    {
        /// <summary>
        /// Asynchronously checks existing record that match the specified filter.
        /// </summary>
        /// <param name="filter">The filter expression to apply to the entity set.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>The true if record is exits otherwise false.</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during the check exists operation.</exception>
        public async Task<bool> IsExistsAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default)
        {
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(CheckExistTimeout));
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                IQueryable<TEntity> query = _dbSet;

                var result = await query.Where(filter).AnyAsync(cancellationToken);
                return result;
            }
            catch (OperationCanceledException ex)
            {
                throw new EntityException("The operation was cancelled due to waiting too long for completion or due to manual cancellation", ex);
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to check existing", ex);
            }
        }

        /// <summary>
        /// Checks existing record that match the specified filter.
        /// </summary>
        /// <param name="filter">The filter expression to apply to the entity set.</param>
        /// <returns>The true if record is exits otherwise false.</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during the check exists operation.</exception>
        public bool IsExists(Expression<Func<TEntity, bool>> filter)
        {
            try
            {
                IQueryable<TEntity> query = _dbSet;

                var result = query.Where(filter).Any();
                return result;
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to check existing", ex);
            }
        }

        /// <summary>
        /// Asynchronously retrieves the count of entities that match the specified filter.
        /// </summary>
        /// <param name="filter">The filter expression to apply to the entity set.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>The count of entities that match the filter.</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during the count operation.</exception>
        public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? filter, CancellationToken cancellationToken = default)
        {
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(GetCountTimeout));
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                IQueryable<TEntity> query = _dbSet;
                if (filter is not null)
                    query = query.Where(filter);

                var result = await query.CountAsync(cancellationToken);
                return result;
            }
            catch (OperationCanceledException ex)
            {
                throw new EntityException("The operation was cancelled due to waiting too long for completion or due to manual cancellation", ex);
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to retrieve count of entities", ex);
            }
        }

        /// <summary>
        /// Retrieves the count of entities that match the specified filter.
        /// </summary>
        /// <param name="filter">The filter expression to apply to the entity set.</param>
        /// <returns>The count of entities that match the filter.</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during the count operation.</exception>
        public int GetCount(Expression<Func<TEntity, bool>>? filter)
        {
            try
            {
                IQueryable<TEntity> query = _dbSet;
                if (filter is not null)
                    query = query.Where(filter);

                var result = query.Count();
                return result;
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to retrieve count of entities", ex);
            }
        }
    }

    public partial class Repository<TEntity, TDbContext>
    {
        /// <summary>
        /// Asynchronously retrieves an entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity to retrieve.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>The entity with the specified identifier, or <c>null</c> if not found.</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during the retrieval operation.</exception>
        public async Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(GetByIdTimeout));
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                return await _dbSet.FindAsync([id, cancellationToken], cancellationToken: cancellationToken);
            }
            catch (OperationCanceledException ex)
            {
                throw new EntityException("The operation was cancelled due to waiting too long for completion or due to manual cancellation", ex);
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to retrieve an entity by its ID", ex);
            }
        }

        /// <summary>
        /// Retrieves an entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity to retrieve.</param>
        /// <returns>The entity with the specified identifier, or <c>null</c> if not found.</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during the retrieval operation.</exception>
        public TEntity? GetById(object id)
        {
            try
            {
                return _dbSet.Find(id);
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to retrieve an entity by its ID", ex);
            }
        }

        /// <summary>
        /// Asynchronously retrieves the first or last entity that matches the specified filter, including options for ordering and tracking.
        /// </summary>
        /// <param name="builder">A <see cref="SingleQueryBuilder{TEntity}"/> that defines the query criteria.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>The first or last entity that matches the filter, or <c>null</c> if not found.</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during the retrieval operation.</exception>
        public async Task<TEntity?> GetSingleAsync(SingleQueryBuilder<TEntity> builder, CancellationToken cancellationToken = default)
        {
            try
            {
                var timeout = builder.Timeout == TimeSpan.Zero ? TimeSpan.FromSeconds(GetSingleTimeout) : builder.Timeout;
                using var cts = new CancellationTokenSource(timeout);
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                IQueryable<TEntity> query = _dbSet;

                query = query.ApplyBuilder(builder);
                return builder.TakeFirst ? await query.FirstOrDefaultAsync(cancellationToken) : await query.LastOrDefaultAsync(cancellationToken);
            }
            catch (OperationCanceledException ex)
            {
                throw new EntityException("The operation was cancelled due to waiting too long for completion or due to manual cancellation", ex);
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to retrieve an entity using a single query builder", ex);
            }
        }

        /// <summary>
        /// Asynchronously retrieves an entity by configuring the query builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the query builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The found entity or null</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during the retrieval operation</exception>
        public async Task<TEntity?> GetSingleAsync(Action<SingleQueryBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default)
        {
            var builder = new SingleQueryBuilder<TEntity>();
            builderAction(builder);
            return await GetSingleAsync(builder, cancellationToken);
        }

        /// <summary>
        /// Retrieves the first or last entity that matches the specified filter, including options for ordering and tracking.
        /// </summary>
        /// <param name="builder">A <see cref="SingleQueryBuilder{TEntity}"/> that defines the query criteria.</param>
        /// <returns>The first or last entity that matches the filter, or <c>null</c> if not found.</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during the retrieval operation.</exception>
        public TEntity? GetSingle(SingleQueryBuilder<TEntity> builder)
        {
            try
            {
                IQueryable<TEntity> query = _dbSet;

                query = query.ApplyBuilder(builder);
                return builder.TakeFirst ? query.FirstOrDefault() : query.LastOrDefault();
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to retrieve an entity using a single query builder", ex);
            }
        }

        /// <summary>
        /// Retrieves an entity by configuring the query builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the query builder</param>
        /// <returns>The found entity or null</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during the retrieval operation</exception>
        public TEntity? GetSingle(Action<SingleQueryBuilder<TEntity>> builderAction)
        {
            var builder = new SingleQueryBuilder<TEntity>();
            builderAction(builder);
            return GetSingle(builder);
        }

        /// <summary>
        /// Asynchronously retrieves a range of entities based on the specified query builder.
        /// </summary>
        /// <param name="builder">A <see cref="RangeQueryBuilder{TEntity}"/> that defines the query criteria.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>A list of entities matching the query criteria.</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during the retrieval operation.</exception>
        public async Task<IEnumerable<TEntity>> GetRangeAsync(RangeQueryBuilder<TEntity>? builder, CancellationToken cancellationToken = default)
        {
            try
            {
                var timeout = builder?.Timeout == TimeSpan.Zero || builder is null ? TimeSpan.FromSeconds(GetRangeTimeout) : builder.Timeout;
                using var cts = new CancellationTokenSource(timeout);
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                IQueryable<TEntity> query = _dbSet;

                if (builder is null)
                    return await query.ToArrayAsync(cancellationToken);

                return await query.ApplyBuilder(builder).ToArrayAsync(cancellationToken);
            }
            catch (OperationCanceledException ex)
            {
                throw new EntityException("The operation was cancelled due to waiting too long for completion or due to manual cancellation", ex);
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to retrieve a range of entities", ex);
            }
        }

        /// <summary>
        /// Asynchronously retrieves multiple entities by configuring the query builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the query builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A collection of entities</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during the retrieval operation</exception>
        public async Task<IEnumerable<TEntity>> GetRangeAsync(Action<RangeQueryBuilder<TEntity>>? builderAction, CancellationToken cancellationToken = default)
        {
            if (builderAction == null)
                return await GetRangeAsync((RangeQueryBuilder<TEntity>?)null, cancellationToken);

            var builder = new RangeQueryBuilder<TEntity>();
            builderAction(builder);
            return await GetRangeAsync(builder, cancellationToken);
        }

        /// <summary>
        /// Retrieves a range of entities based on the specified query builder.
        /// </summary>
        /// <param name="builder">A <see cref="RangeQueryBuilder{TEntity}"/> that defines the query criteria.</param>
        /// <returns>A list of entities matching the query criteria.</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during the retrieval operation.</exception>
        public IEnumerable<TEntity> GetRange(RangeQueryBuilder<TEntity>? builder)
        {
            try
            {
                IQueryable<TEntity> query = _dbSet;

                if (builder is null)
                    return query.ToArray();

                return query.ApplyBuilder(builder).ToArray();
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to retrieve a range of entities", ex);
            }
        }

        /// <summary>
        /// Retrieves multiple entities by configuring the query builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the query builder</param>
        /// <returns>A collection of entities</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during the retrieval operation</exception>
        public IEnumerable<TEntity> GetRange(Action<RangeQueryBuilder<TEntity>>? builderAction)
        {
            if (builderAction == null)
                return GetRange((RangeQueryBuilder<TEntity>?)null);

            var builder = new RangeQueryBuilder<TEntity>();
            builderAction(builder);
            return GetRange(builder);
        }

        /// <summary>
        /// Asynchronously retrieves a range of entities with total count.
        /// </summary>
        /// <param name="builder">A <see cref="RangeQueryBuilder{TEntity}"/> that defines the query criteria.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A <see cref="EntityChunkDetails{TEntity}"/> chunk of entities with total count.</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during the retrieval operation.</exception>
        public async Task<EntityChunkDetails<TEntity>> GetRangeEntireAsync(RangeQueryBuilder<TEntity>? builder, CancellationToken cancellationToken = default)
        {
            try
            {
                var timeout = builder?.Timeout == TimeSpan.Zero || builder is null ? TimeSpan.FromSeconds(GetRangeTimeout) : builder.Timeout;
                using var cts = new CancellationTokenSource(timeout);
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                IEnumerable<TEntity> collection = [];
                IQueryable<TEntity> collectionQuery = _dbSet;
                IQueryable<TEntity> countQuery = _dbSet;

                foreach (var filter in builder?.Filters ?? [])
                    countQuery = countQuery.Where(filter);

                var count = await countQuery.CountAsync(cancellationToken);

                if (builder is null)
                    collection = await collectionQuery.ToArrayAsync(cancellationToken);
                else
                    collection = await collectionQuery.ApplyBuilder(builder).ToArrayAsync(cancellationToken);

                return new EntityChunkDetails<TEntity>
                {
                    Chunk = collection,
                    Total = count,
                };
            }
            catch (OperationCanceledException ex)
            {
                throw new EntityException("The operation was cancelled due to waiting too long for completion or due to manual cancellation", ex);
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to retrieve a range of entities", ex);
            }
        }

        /// <summary>
        /// Asynchronously retrieves a range of entities with total count by configuring the query builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the query builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A chunk of entities with total count</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during the retrieval operation</exception>
        public async Task<EntityChunkDetails<TEntity>> GetRangeEntireAsync(Action<RangeQueryBuilder<TEntity>>? builderAction, CancellationToken cancellationToken = default)
        {
            if (builderAction == null)
                return await GetRangeEntireAsync((RangeQueryBuilder<TEntity>?)null, cancellationToken);

            var builder = new RangeQueryBuilder<TEntity>();
            builderAction(builder);
            return await GetRangeEntireAsync(builder, cancellationToken);
        }

        /// <summary>
        /// Retrieves a range of entities with total count.
        /// </summary>
        /// <param name="builder">A <see cref="RangeQueryBuilder{TEntity}"/> that defines the query criteria.</param>
        /// <returns>A <see cref="EntityChunkDetails{TEntity}"/> chunk of entities with total count.</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during the retrieval operation.</exception>
        public EntityChunkDetails<TEntity> GetRangeEntire(RangeQueryBuilder<TEntity>? builder)
        {
            try
            {
                IEnumerable<TEntity> collection = [];
                IQueryable<TEntity> collectionQuery = _dbSet;
                IQueryable<TEntity> countQuery = _dbSet;

                foreach (var filter in builder?.Filters ?? [])
                    countQuery = countQuery.Where(filter);

                var count = countQuery.Count();

                if (builder is null)
                    collection = collectionQuery.ToArray();
                else
                    collection = collectionQuery.ApplyBuilder(builder).ToArray();

                return new EntityChunkDetails<TEntity>
                {
                    Chunk = collection,
                    Total = count,
                };
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to retrieve a range of entities", ex);
            }
        }

        /// <summary>
        /// Retrieves a range of entities with total count by configuring the query builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the query builder</param>
        /// <returns>A chunk of entities with total count</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during the retrieval operation</exception>
        public EntityChunkDetails<TEntity> GetRangeEntire(Action<RangeQueryBuilder<TEntity>>? builderAction)
        {
            if (builderAction == null)
                return GetRangeEntire((RangeQueryBuilder<TEntity>?)null);

            var builder = new RangeQueryBuilder<TEntity>();
            builderAction(builder);
            return GetRangeEntire(builder);
        }
    }

    public partial class Repository<TEntity, TDbContext>
    {
        /// <summary>
        /// Asynchronously adds a new entity to the repository.
        /// </summary>
        /// <param name="builder">Preconfigured builder containing the entity to add.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>The added entity.</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during the add operation.</exception>
        public async Task<TEntity> AddAsync(AddSingleBuilder<TEntity> builder, CancellationToken cancellationToken = default)
        {
            try
            {
                var timeout = builder.Timeout == TimeSpan.Zero ? TimeSpan.FromSeconds(AddTimeout) : builder.Timeout;
                using var cts = new CancellationTokenSource(timeout);
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                builder.Entity.CreatedBy = builder.CreatedByUser;
                await _dbSet.AddAsync(builder.Entity, cancellationToken);

                if (builder.SaveChanges)
                    await _context.SaveChangesAsync(cancellationToken);

                return builder.Entity;
            }
            catch (OperationCanceledException ex)
            {
                throw new EntityException("The operation was cancelled due to waiting too long for completion or due to manual cancellation", ex);
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to add an entity", ex);
            }
        }

        /// <summary>
        /// Asynchronously adds an entity by configuring the builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the create builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The added entity.</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during the add operation.</exception>
        public async Task<TEntity> AddAsync(Action<AddSingleBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default)
        {
            var builder = new AddSingleBuilder<TEntity>();
            builderAction(builder);
            return await AddAsync(builder, cancellationToken);
        }

        /// <summary>
        /// Adds an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Preconfigured builder containing the entity to add.</param>
        /// <returns>The added entity.</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during the add operation.</exception>
        public TEntity Add(AddSingleBuilder<TEntity> builder)
        {
            try
            {
                builder.Entity.CreatedBy = builder.CreatedByUser;
                _dbSet.Add(builder.Entity);

                if (builder.SaveChanges)
                    _context.SaveChanges();

                return builder.Entity;
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to add an entity", ex);
            }
        }

        /// <summary>
        /// Adds an entity by configuring the builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the create builder</param>
        /// <returns>The added entity.</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during the add operation.</exception>
        public TEntity Add(Action<AddSingleBuilder<TEntity>> builderAction)
        {
            var builder = new AddSingleBuilder<TEntity>();
            builderAction(builder);
            return Add(builder);
        }

        /// <summary>
        /// Asynchronously adds multiple entities to the repository using a configured builder.
        /// </summary>
        /// <param name="builder">Preconfigured builder containing the entities to add.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>The collection of added entities.</returns>
        /// <exception cref="EntityException">
        /// Thrown when an error occurs during the add operation or when operation is canceled.
        /// </exception>
        public async Task<IEnumerable<TEntity>> AddRangeAsync(AddRangeBuilder<TEntity> builder, CancellationToken cancellationToken = default)
        {
            try
            {
                var timeout = builder.Timeout == TimeSpan.Zero ? TimeSpan.FromSeconds(AddRangeTimeout) : builder.Timeout;
                using var cts = new CancellationTokenSource(timeout);
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                foreach (var entity in builder.Entities)
                    entity.CreatedBy = builder.CreatedByUser;

                await _dbSet.AddRangeAsync(builder.Entities, cancellationToken);

                if (builder.SaveChanges)
                    await _context.SaveChangesAsync(cancellationToken);

                return builder.Entities;
            }
            catch (OperationCanceledException ex)
            {
                throw new EntityException("The operation was cancelled due to waiting too long for completion or due to manual cancellation", ex);
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to add a range of entities", ex);
            }
        }

        /// <summary>
        /// Asynchronously adds multiple entities by configuring the builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the create builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The collection of added entities.</returns>
        /// <exception cref="EntityException">
        /// Thrown when an error occurs during the add operation or when operation is canceled.
        /// </exception>
        public async Task<IEnumerable<TEntity>> AddRangeAsync(Action<AddRangeBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default)
        {
            var builder = new AddRangeBuilder<TEntity>();
            builderAction(builder);
            return await AddRangeAsync(builder, cancellationToken);
        }

        /// <summary>
        /// Adds multiple entities to the repository using a configured builder.
        /// </summary>
        /// <param name="builder">Preconfigured builder containing the entities to add.</param>
        /// <returns>The collection of added entities.</returns>
        /// <exception cref="EntityException">
        /// Thrown when an error occurs during the add operation.
        /// </exception>
        public IEnumerable<TEntity> AddRange(AddRangeBuilder<TEntity> builder)
        {
            try
            {
                foreach (var entity in builder.Entities)
                    entity.CreatedBy = builder.CreatedByUser;

                _dbSet.AddRange(builder.Entities);

                if (builder.SaveChanges)
                    _context.SaveChanges();

                return builder.Entities;
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to add a range of entities", ex);
            }
        }

        /// <summary>
        /// Adds multiple entities by configuring the builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the create builder</param>
        /// <returns>A collection of added entities.</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during the add operation.</exception>
        public IEnumerable<TEntity> AddRange(Action<AddRangeBuilder<TEntity>> builderAction)
        {
            var builder = new AddRangeBuilder<TEntity>();
            builderAction(builder);
            return AddRange(builder);
        }
    }

    public partial class Repository<TEntity, TDbContext>
    {
        /// <summary>
        /// Asynchronously removes an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Builder with entity remove parameters</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed</param>
        /// <returns>The removed entity, or null if not found</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during removal</exception>
        public async Task<TEntity?> RemoveAsync(RemoveSingleBuilder<TEntity> builder, CancellationToken cancellationToken = default)
        {
            try
            {
                var timeout = builder.Timeout == TimeSpan.Zero ? TimeSpan.FromSeconds(RemoveTimeout) : builder.Timeout;
                using var cts = new CancellationTokenSource(timeout);
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                var entity = await GetEntityToRemoveAsync(builder, cancellationToken);

                if (entity is null)
                    return null;

                var deletedEntity = _dbSet.Remove(entity).Entity;

                if (builder.SaveChanges)
                    await _context.SaveChangesAsync(cancellationToken);

                return deletedEntity;
            }
            catch (OperationCanceledException ex)
            {
                throw new EntityException("The operation was cancelled due to waiting too long for completion or due to manual cancellation", ex);
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to delete an entity", ex);
            }
        }

        /// <summary>
        /// Asynchronously removes an entity by configuring the remove builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the remove builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The removed entity or null.</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during removal</exception>
        public async Task<TEntity?> RemoveAsync(Action<RemoveSingleBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default)
        {
            var builder = new RemoveSingleBuilder<TEntity>();
            builderAction(builder);
            return await RemoveAsync(builder, cancellationToken);
        }

        /// <summary>
        /// Removes an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Builder with entity remove parameters</param>
        /// <returns>The removed entity, or null if not found</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during removal</exception>
        public TEntity? Remove(RemoveSingleBuilder<TEntity> builder)
        {
            try
            {
                var entity = GetEntityToRemove(builder);

                if (entity is null)
                    return null;

                var deletedEntity = _dbSet.Remove(entity).Entity;

                if (builder.SaveChanges)
                    _context.SaveChanges();

                return deletedEntity;
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to delete an entity", ex);
            }
        }

        /// <summary>
        /// Removes an entity by configuring the remove builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the remove builder</param>
        /// <returns>The removed entity or null.</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during removal</exception>
        public TEntity? Remove(Action<RemoveSingleBuilder<TEntity>> builderAction)
        {
            var builder = new RemoveSingleBuilder<TEntity>();
            builderAction(builder);
            return Remove(builder);
        }

        /// <summary>
        /// Asynchronously removes multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured builder with remove parameters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Removed entities</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during removal</exception>
        public async Task<IEnumerable<TEntity>> RemoveRangeAsync(RemoveRangeBuilder<TEntity> builder, CancellationToken cancellationToken = default)
        {
            try
            {
                var timeout = builder.Timeout == TimeSpan.Zero ? TimeSpan.FromSeconds(RemoveRangeTimeout) : builder.Timeout;
                using var cts = new CancellationTokenSource(timeout);
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                var entities = await GetEntitiesToRemoveAsync(builder, cancellationToken);

                _context.RemoveRange(entities);

                if (builder.SaveChanges)
                    await _context.SaveChangesAsync(cancellationToken);

                return entities;
            }
            catch (OperationCanceledException ex)
            {
                throw new EntityException("The operation was cancelled due to waiting too long for completion or due to manual cancellation", ex);
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to delete a range of entities", ex);
            }
        }

        /// <summary>
        /// Asynchronously removes multiple entities by configuring the remove builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the remove builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A collection of removed entities.</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during removal</exception>
        public async Task<IEnumerable<TEntity>> RemoveRangeAsync(Action<RemoveRangeBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default)
        {
            var builder = new RemoveRangeBuilder<TEntity>();
            builderAction(builder);
            return await RemoveRangeAsync(builder, cancellationToken);
        }

        /// <summary>
        /// Removes multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured builder with remove parameters</param>
        /// <returns>Removed entities</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during removal</exception>
        public IEnumerable<TEntity> RemoveRange(RemoveRangeBuilder<TEntity> builder)
        {
            try
            {
                var entities = GetEntitiesToRemove(builder);

                _context.RemoveRange(entities);

                if (builder.SaveChanges)
                    _context.SaveChanges();

                return entities;
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to delete a range of entities", ex);
            }
        }

        /// <summary>
        /// Removes multiple entities by configuring the remove builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the remove builder</param>
        /// <returns>A collection of removed entities.</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during removal</exception>
        public IEnumerable<TEntity> RemoveRange(Action<RemoveRangeBuilder<TEntity>> builderAction)
        {
            var builder = new RemoveRangeBuilder<TEntity>();
            builderAction(builder);
            return RemoveRange(builder);
        }

        /// <summary>
        /// Retrieves an entity to be removed based on the specified remove mode in the builder.
        /// </summary>
        /// <param name="builder">The remove builder containing the removal configuration.</param>
        /// <returns>
        /// The entity to be removed:
        /// - For <see cref="EntityRemoveMode.Entity"/>: returns the entity directly from the builder
        /// - For <see cref="EntityRemoveMode.Identifier"/>: finds the entity by ID in the database
        /// - For <see cref="EntityRemoveMode.Filter"/>: finds the first entity matching the filter
        /// Returns null if no matching entity is found or if the configuration is invalid.
        /// </returns>
        private TEntity? GetEntityToRemove(RemoveSingleBuilder<TEntity> builder)
        {
            return builder.RemoveMode switch
            {
                EntityRemoveMode.Entity when builder.Entity is not null => builder.Entity,
                EntityRemoveMode.Identifier when builder.Id is not null => _dbSet.Find(builder.Id),
                EntityRemoveMode.Filter when builder.Filter is not null => _dbSet.FirstOrDefault(builder.Filter),
                _ => null
            };
        }

        /// <summary>
        /// Asynchronously retrieves an entity to be removed based on the specified remove mode in the builder.
        /// </summary>
        /// <param name="builder">The remove builder containing the removal configuration.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains:
        /// - For <see cref="EntityRemoveMode.Entity"/>: the entity directly from the builder
        /// - For <see cref="EntityRemoveMode.Identifier"/>: the entity found by ID in the database
        /// - For <see cref="EntityRemoveMode.Filter"/>: the first entity matching the filter
        /// The result is null if no matching entity is found or if the configuration is invalid.
        /// </returns>
        private async Task<TEntity?> GetEntityToRemoveAsync(RemoveSingleBuilder<TEntity> builder, CancellationToken cancellationToken)
        {
            return builder.RemoveMode switch
            {
                EntityRemoveMode.Entity when builder.Entity is not null => builder.Entity,
                EntityRemoveMode.Identifier when builder.Id is not null => await _dbSet.FindAsync([builder.Id, cancellationToken], cancellationToken: cancellationToken),
                EntityRemoveMode.Filter when builder.Filter is not null => await _dbSet.FirstOrDefaultAsync(builder.Filter, cancellationToken),
                _ => null
            };
        }

        /// <summary>
        /// Retrieves a collection of entities to be removed based on the specified remove mode in the builder.
        /// </summary>
        /// <param name="builder">The remove builder containing the removal configuration.</param>
        /// <returns>
        /// The collection of entities to be removed:
        /// - For <see cref="EntityRemoveMode.Entity"/>: returns the entities directly from the builder
        /// - For <see cref="EntityRemoveMode.Identifier"/>: finds entities matching any of the specified IDs
        /// - For <see cref="EntityRemoveMode.Filter"/>: finds all entities matching the filter
        /// </returns>
        private IEnumerable<TEntity> GetEntitiesToRemove(RemoveRangeBuilder<TEntity> builder)
        {
            return builder.RemoveMode switch
            {
                EntityRemoveMode.Entity => builder.Entities,
                EntityRemoveMode.Identifier => _dbSet.Where(e => builder.Identifiers.Contains(e.Id)).ToArray(),
                EntityRemoveMode.Filter when builder.Filter is not null => _dbSet.Where(builder.Filter).ToArray(),
                _ => throw new EntityException($"Invalid {nameof(builder.RemoveMode)}")
            };
        }

        /// <summary>
        /// Asynchronously retrieves a collection of entities to be removed based on the specified remove mode in the builder.
        /// </summary>
        /// <param name="builder">The remove builder containing the removal configuration.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains:
        /// - For <see cref="EntityRemoveMode.Entity"/>: the entities directly from the builder
        /// - For <see cref="EntityRemoveMode.Identifier"/>: entities matching any of the specified IDs
        /// - For <see cref="EntityRemoveMode.Filter"/>: all entities matching the filter
        /// </returns>
        private async Task<IEnumerable<TEntity>> GetEntitiesToRemoveAsync(RemoveRangeBuilder<TEntity> builder, CancellationToken cancellationToken)
        {
            return builder.RemoveMode switch
            {
                EntityRemoveMode.Entity => builder.Entities,
                EntityRemoveMode.Identifier => await _dbSet.Where(e => builder.Identifiers.Contains(e.Id)).ToArrayAsync(cancellationToken),
                EntityRemoveMode.Filter when builder.Filter is not null => await _dbSet.Where(builder.Filter).ToArrayAsync(cancellationToken),
                _ => throw new EntityException($"Invalid {nameof(builder.RemoveMode)}")
            };
        }
    }

    public partial class Repository<TEntity, TDbContext>
    {
        /// <summary>
        /// Asynchronously updates an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Builder with entity update parameters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The updated entity</returns>
        /// <exception cref="EntityException">On update error</exception>
        public async Task<TEntity> UpdateAsync(UpdateSingleBuilder<TEntity> builder, CancellationToken cancellationToken = default)
        {
            try
            {
                var timeout = builder.Timeout == TimeSpan.Zero ? TimeSpan.FromSeconds(UpdateTimeout) : builder.Timeout;
                using var cts = new CancellationTokenSource(timeout);
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                var entity = builder.Entity;
                var entry = _dbSet.Entry(entity);

                if (entry.State != EntityState.Detached)
                    entity.UpdatedBy = builder.UpdatedByUser;
                else
                {
                    entity.UpdatedBy = builder.UpdatedByUser;
                    _dbSet.Update(entity);
                }

                if (builder.SaveChanges)
                    await _context.SaveChangesAsync(cancellationToken);

                return entity;
            }
            catch (OperationCanceledException ex)
            {
                throw new EntityException("The operation was cancelled due to waiting too long for completion or due to manual cancellation", ex);
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to update an entity", ex);
            }
        }

        /// <summary>
        /// Asynchronously updates an entity by configuring the update builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the update builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The updated entity</returns>
        /// <exception cref="EntityException">On update error</exception>
        public async Task<TEntity> UpdateAsync(Action<UpdateSingleBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default)
        {
            var builder = new UpdateSingleBuilder<TEntity>();
            builderAction(builder);
            return await UpdateAsync(builder, cancellationToken);
        }

        /// <summary>
        /// Updates an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Builder with entity update parameters</param>
        /// <returns>The updated entity</returns>
        /// <exception cref="EntityException">On update error</exception>
        public TEntity Update(UpdateSingleBuilder<TEntity> builder)
        {
            try
            {
                var entity = builder.Entity;
                var entry = _dbSet.Entry(entity);

                if (entry.State != EntityState.Detached)
                    entity.UpdatedBy = builder.UpdatedByUser;
                else
                {
                    entity.UpdatedBy = builder.UpdatedByUser;
                    _dbSet.Update(entity);
                }

                if (builder.SaveChanges)
                    _context.SaveChanges();

                return entity;
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to update an entity", ex);
            }
        }

        /// <summary>
        /// Updates an entity by configuring the update builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the update builder</param>
        /// <returns>The updated entity</returns>
        /// <exception cref="EntityException">On update error</exception>
        public TEntity Update(Action<UpdateSingleBuilder<TEntity>> builderAction)
        {
            var builder = new UpdateSingleBuilder<TEntity>();
            builderAction(builder);
            return Update(builder);
        }

        /// <summary>
        /// Asynchronously updates multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured builder with update parameters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated entities</returns>
        /// <exception cref="EntityException">On update error</exception>
        public async Task<IEnumerable<TEntity>> UpdateRangeAsync(UpdateRangeBuilder<TEntity> builder, CancellationToken cancellationToken = default)
        {
            try
            {
                var timeout = builder.Timeout == TimeSpan.Zero ? TimeSpan.FromSeconds(UpdateRangeTimeout) : builder.Timeout;
                using var cts = new CancellationTokenSource(timeout);
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                foreach (var entity in builder.Entities)
                {
                    var entry = _dbSet.Entry(entity);

                    if (entry.State != EntityState.Detached)
                        entity.UpdatedBy = builder.UpdatedByUser;
                    else
                    {
                        entity.UpdatedBy = builder.UpdatedByUser;
                        _dbSet.Update(entity);
                    }
                }

                if (builder.SaveChanges)
                    await _context.SaveChangesAsync(cancellationToken);

                return builder.Entities;
            }
            catch (OperationCanceledException ex)
            {
                throw new EntityException("The operation was cancelled due to waiting too long for completion or due to manual cancellation", ex);
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to update a range of entities", ex);
            }
        }

        /// <summary>
        /// Asynchronously updates multiple entities by configuring the update builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the update builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated entities</returns>
        /// <exception cref="EntityException">On update error</exception>
        public async Task<IEnumerable<TEntity>> UpdateRangeAsync(Action<UpdateRangeBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default)
        {
            var builder = new UpdateRangeBuilder<TEntity>();
            builderAction(builder);
            return await UpdateRangeAsync(builder, cancellationToken);
        }

        /// <summary>
        /// Updates multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured builder with update parameters</param>
        /// <returns>Updated entities</returns>
        /// <exception cref="EntityException">On update error</exception>
        public IEnumerable<TEntity> UpdateRange(UpdateRangeBuilder<TEntity> builder)
        {
            try
            {
                foreach (var entity in builder.Entities)
                {
                    var entry = _dbSet.Entry(entity);

                    if (entry.State != EntityState.Detached)
                        entity.UpdatedBy = builder.UpdatedByUser;
                    else
                    {
                        entity.UpdatedBy = builder.UpdatedByUser;
                        _dbSet.Update(entity);
                    }
                }

                if (builder.SaveChanges)
                    _context.SaveChanges();

                return builder.Entities;
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to update a range of entities", ex);
            }
        }

        /// <summary>
        /// Updates multiple entities by configuring the update builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the update builder</param>
        /// <returns>Updated entities</returns>
        /// <exception cref="EntityException">On update error</exception>
        public IEnumerable<TEntity> UpdateRange(Action<UpdateRangeBuilder<TEntity>> builderAction)
        {
            var builder = new UpdateRangeBuilder<TEntity>();
            builderAction(builder);
            return UpdateRange(builder);
        }
    }

    public partial class Repository<TEntity, TDbContext>
    {
        /// <summary>
        /// Asynchronously restores a soft-deleted entity using a configured builder.
        /// </summary>
        /// <param name="builder">Builder with entity restore parameters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The restored entity</returns>
        /// <exception cref="EntityException">On restore error</exception>
        public async Task<TEntity> RestoreAsync(RestoreSingleBuilder<TEntity> builder, CancellationToken cancellationToken = default)
        {
            try
            {
                var timeout = builder.Timeout == TimeSpan.Zero ? TimeSpan.FromSeconds(RestoreTimeout) : builder.Timeout;
                using var cts = new CancellationTokenSource(timeout);
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                var entity = builder.Entity;
                var entry = _dbSet.Entry(entity);

                if (entry.State != EntityState.Detached)
                    entity.IsDeleted = false;
                else
                {
                    entity.IsDeleted = false;
                    _dbSet.Update(entity);
                }

                if (builder.SaveChanges)
                    await _context.SaveChangesAsync(cancellationToken);

                return builder.Entity;
            }
            catch (OperationCanceledException ex)
            {
                throw new EntityException("The operation was cancelled due to waiting too long for completion or due to manual cancellation", ex);
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to restore a soft-deleted entity", ex);
            }
        }

        /// <summary>
        /// Asynchronously restores a soft-deleted entity by configuring the restore builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the restore builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The restored entity</returns>
        /// <exception cref="EntityException">On restore error</exception>
        public async Task<TEntity> RestoreAsync(Action<RestoreSingleBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default)
        {
            var builder = new RestoreSingleBuilder<TEntity>();
            builderAction(builder);
            return await RestoreAsync(builder, cancellationToken);
        }

        /// <summary>
        /// Restores a soft-deleted entity using a configured builder.
        /// </summary>
        /// <param name="builder">Builder with entity restore parameters</param>
        /// <returns>The restored entity</returns>
        /// <exception cref="EntityException">On restore error</exception>
        public TEntity Restore(RestoreSingleBuilder<TEntity> builder)
        {
            try
            {
                var entity = builder.Entity;
                var entry = _dbSet.Entry(entity);

                if (entry.State != EntityState.Detached)
                    entity.IsDeleted = false;
                else
                {
                    entity.IsDeleted = false;
                    _dbSet.Update(entity);
                }

                if (builder.SaveChanges)
                    _context.SaveChanges();

                return builder.Entity;
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to restore a soft-deleted entity", ex);
            }
        }

        /// <summary>
        /// Restores a soft-deleted entity by configuring the restore builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the restore builder</param>
        /// <returns>The restored entity</returns>
        /// <exception cref="EntityException">On restore error</exception>
        public TEntity Restore(Action<RestoreSingleBuilder<TEntity>> builderAction)
        {
            var builder = new RestoreSingleBuilder<TEntity>();
            builderAction(builder);
            return Restore(builder);
        }

        /// <summary>
        /// Asynchronously restores multiple soft-deleted entities using a configured builder.
        /// </summary>
        /// <param name="builder">Builder with entities to restore</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Restored entities</returns>
        /// <exception cref="EntityException">On restore error</exception>
        public async Task<IEnumerable<TEntity>> RestoreRangeAsync(RestoreRangeBuilder<TEntity> builder, CancellationToken cancellationToken = default)
        {
            try
            {
                var timeout = builder.Timeout == TimeSpan.Zero ? TimeSpan.FromSeconds(RestoreRangeTimeout) : builder.Timeout;
                using var cts = new CancellationTokenSource(timeout);
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                foreach (var entity in builder.Entities)
                {
                    var entry = _dbSet.Entry(entity);

                    if (entry.State != EntityState.Detached)
                        entity.IsDeleted = false;
                    else
                    {
                        entity.IsDeleted = false;
                        _dbSet.Update(entity);
                    }
                }

                if (builder.SaveChanges)
                    await _context.SaveChangesAsync(cancellationToken);

                return builder.Entities;
            }
            catch (OperationCanceledException ex)
            {
                throw new EntityException("The operation was cancelled due to waiting too long for completion or due to manual cancellation", ex);
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to restore a range of soft-deleted entities", ex);
            }
        }

        /// <summary>
        /// Asynchronously restores multiple soft-deleted entities by configuring the restore builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the restore builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Restored entities</returns>
        /// <exception cref="EntityException">On restore error</exception>
        public async Task<IEnumerable<TEntity>> RestoreRangeAsync(Action<RestoreRangeBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default)
        {
            var builder = new RestoreRangeBuilder<TEntity>();
            builderAction(builder);
            return await RestoreRangeAsync(builder, cancellationToken);
        }

        /// <summary>
        /// Restores multiple soft-deleted entities using a configured builder.
        /// </summary>
        /// <param name="builder">Builder with entities to restore</param>
        /// <returns>Restored entities</returns>
        /// <exception cref="EntityException">On restore error</exception>
        public IEnumerable<TEntity> RestoreRange(RestoreRangeBuilder<TEntity> builder)
        {
            try
            {
                foreach (var entity in builder.Entities)
                {
                    var entry = _dbSet.Entry(entity);

                    if (entry.State != EntityState.Detached)
                        entity.IsDeleted = false;
                    else
                    {
                        entity.IsDeleted = false;
                        _dbSet.Update(entity);
                    }
                }

                if (builder.SaveChanges)
                    _context.SaveChanges();

                return builder.Entities;
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to restore a range of soft-deleted entities", ex);
            }
        }

        /// <summary>
        /// Restores multiple soft-deleted entities by configuring the restore builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the restore builder</param>
        /// <returns>Restored entities</returns>
        /// <exception cref="EntityException">On restore error</exception>
        public IEnumerable<TEntity> RestoreRange(Action<RestoreRangeBuilder<TEntity>> builderAction)
        {
            var builder = new RestoreRangeBuilder<TEntity>();
            builderAction(builder);
            return RestoreRange(builder);
        }
    }
}
