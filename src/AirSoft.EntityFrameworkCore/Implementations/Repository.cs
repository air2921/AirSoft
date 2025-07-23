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

        /// <summary>Timeout for GetCount operations (10 seconds).</summary>
        private const int GetCountTimeout = 10;

        /// <summary>Timeout for GetRange operations (20 seconds).</summary>
        private const int GetRangeTimeout = 20;

        /// <summary>Timeout for GetSingle operations (15 seconds).</summary>
        private const int GetSingleTimeout = 15;

        /// <summary>Timeout for GetById operations (10 seconds).</summary>
        private const int GetByIdTimeout = 10;

        /// <summary>Timeout for Add operations (20 seconds).</summary>
        private const int AddTimeout = 20;

        /// <summary>Timeout for AddRange operations (30 seconds).</summary>
        private const int AddRangeTimeout = 30;

        /// <summary>Timeout for RemoveRange operations (30 seconds).</summary>
        private const int RemoveRangeTimeout = 30;

        /// <summary>Timeout for Remove operations (20 seconds).</summary>
        private const int RemoveTimeout = 20;

        /// <summary>Timeout for Update operations (20 seconds).</summary>
        private const int UpdateTimeout = 20;

        /// <summary>Timeout for UpdateRange operations (30 seconds).</summary>
        private const int UpdateRangeTimeout = 30;

        /// <summary>Timeout for Restore operations (20 seconds).</summary>
        private const int RestoreTimeout = 20;

        /// <summary>Timeout for RestoreRange operations (20 seconds).</summary>
        private const int RestoreRangeTimeout = 20;

        #endregion
    }

    public partial class Repository<TEntity, TDbContext>
    {
        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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
        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public async Task<TEntity?> GetSingleAsync(Action<SingleQueryBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default)
        {
            var builder = new SingleQueryBuilder<TEntity>();
            builderAction(builder);
            return await GetSingleAsync(builder, cancellationToken);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public TEntity? GetSingle(Action<SingleQueryBuilder<TEntity>> builderAction)
        {
            var builder = new SingleQueryBuilder<TEntity>();
            builderAction(builder);
            return GetSingle(builder);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public async Task<IEnumerable<TEntity>> GetRangeAsync(Action<RangeQueryBuilder<TEntity>>? builderAction, CancellationToken cancellationToken = default)
        {
            if (builderAction == null)
                return await GetRangeAsync((RangeQueryBuilder<TEntity>?)null, cancellationToken);

            var builder = new RangeQueryBuilder<TEntity>();
            builderAction(builder);
            return await GetRangeAsync(builder, cancellationToken);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetRange(Action<RangeQueryBuilder<TEntity>>? builderAction)
        {
            if (builderAction == null)
                return GetRange((RangeQueryBuilder<TEntity>?)null);

            var builder = new RangeQueryBuilder<TEntity>();
            builderAction(builder);
            return GetRange(builder);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public async Task<EntityChunkDetails<TEntity>> GetRangeEntireAsync(Action<RangeQueryBuilder<TEntity>>? builderAction, CancellationToken cancellationToken = default)
        {
            if (builderAction == null)
                return await GetRangeEntireAsync((RangeQueryBuilder<TEntity>?)null, cancellationToken);

            var builder = new RangeQueryBuilder<TEntity>();
            builderAction(builder);
            return await GetRangeEntireAsync(builder, cancellationToken);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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
        /// <inheritdoc/>
        public async Task<int> AddAsync(AddSingleBuilder<TEntity> builder, CancellationToken cancellationToken = default)
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

                return 1;
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

        /// <inheritdoc/>
        public async Task<int> AddAsync(Action<AddSingleBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default)
        {
            var builder = new AddSingleBuilder<TEntity>();
            builderAction(builder);
            return await AddAsync(builder, cancellationToken);
        }

        /// <inheritdoc/>
        public int Add(AddSingleBuilder<TEntity> builder)
        {
            try
            {
                builder.Entity.CreatedBy = builder.CreatedByUser;
                _dbSet.Add(builder.Entity);

                if (builder.SaveChanges)
                    _context.SaveChanges();

                return 1;
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to add an entity", ex);
            }
        }

        /// <inheritdoc/>
        public int Add(Action<AddSingleBuilder<TEntity>> builderAction)
        {
            var builder = new AddSingleBuilder<TEntity>();
            builderAction(builder);
            return Add(builder);
        }

        /// <inheritdoc/>
        public async Task<int> AddRangeAsync(AddRangeBuilder<TEntity> builder, CancellationToken cancellationToken = default)
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

                return builder.Entities.Count;
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

        /// <inheritdoc/>
        public async Task<int> AddRangeAsync(Action<AddRangeBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default)
        {
            var builder = new AddRangeBuilder<TEntity>();
            builderAction(builder);
            return await AddRangeAsync(builder, cancellationToken);
        }

        /// <inheritdoc/>
        public int AddRange(AddRangeBuilder<TEntity> builder)
        {
            try
            {
                foreach (var entity in builder.Entities)
                    entity.CreatedBy = builder.CreatedByUser;

                _dbSet.AddRange(builder.Entities);

                if (builder.SaveChanges)
                    _context.SaveChanges();

                return builder.Entities.Count;
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to add a range of entities", ex);
            }
        }

        /// <inheritdoc/>
        public int AddRange(Action<AddRangeBuilder<TEntity>> builderAction)
        {
            var builder = new AddRangeBuilder<TEntity>();
            builderAction(builder);
            return AddRange(builder);
        }
    }

    public partial class Repository<TEntity, TDbContext>
    {
        /// <inheritdoc/>
        public async Task<int> RemoveAsync(RemoveSingleBuilder<TEntity> builder, CancellationToken cancellationToken = default)
        {
            try
            {
                var timeout = builder.Timeout == TimeSpan.Zero ? TimeSpan.FromSeconds(RemoveTimeout) : builder.Timeout;
                using var cts = new CancellationTokenSource(timeout);
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                if (builder.IsExeсutable)
                {
                    var query = SetQueryForRemoveExecution(builder);
                    return await query.ExecuteDeleteAsync(cancellationToken);
                }
                else
                {
                    var entity = await GetEntityToRemoveAsync(builder, cancellationToken);

                    if (entity is null)
                        return 0;

                    var deletedEntity = _dbSet.Remove(entity).Entity;

                    if (builder.SaveChanges)
                        await _context.SaveChangesAsync(cancellationToken);

                    return 1;
                }
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

        /// <inheritdoc/>
        public async Task<int> RemoveAsync(Action<RemoveSingleBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default)
        {
            var builder = new RemoveSingleBuilder<TEntity>();
            builderAction(builder);
            return await RemoveAsync(builder, cancellationToken);
        }

        /// <inheritdoc/>
        public int Remove(RemoveSingleBuilder<TEntity> builder)
        {
            try
            {
                if (builder.IsExeсutable)
                {
                    var query = SetQueryForRemoveExecution(builder);
                    return query.ExecuteDelete();
                }
                else
                {
                    var entity = GetEntityToRemove(builder);

                    if (entity is null)
                        return 0;

                    var deletedEntity = _dbSet.Remove(entity).Entity;

                    if (builder.SaveChanges)
                        _context.SaveChanges();

                    return 1;
                }
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to delete an entity", ex);
            }
        }

        /// <inheritdoc/>
        public int Remove(Action<RemoveSingleBuilder<TEntity>> builderAction)
        {
            var builder = new RemoveSingleBuilder<TEntity>();
            builderAction(builder);
            return Remove(builder);
        }

        /// <inheritdoc/>
        public async Task<int> RemoveRangeAsync(RemoveRangeBuilder<TEntity> builder, CancellationToken cancellationToken = default)
        {
            try
            {
                var timeout = builder.Timeout == TimeSpan.Zero ? TimeSpan.FromSeconds(RemoveRangeTimeout) : builder.Timeout;
                using var cts = new CancellationTokenSource(timeout);
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                if (builder.IsExeсutable)
                {
                    var query = SetQueryForRemoveExecution(builder);
                    return await query.ExecuteDeleteAsync(cancellationToken);
                }
                else
                {
                    var entities = await GetEntitiesToRemoveAsync(builder, cancellationToken);

                    _context.RemoveRange(entities);

                    if (builder.SaveChanges)
                        await _context.SaveChangesAsync(cancellationToken);

                    return entities.Count();
                }
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

        /// <inheritdoc/>
        public async Task<int> RemoveRangeAsync(Action<RemoveRangeBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default)
        {
            var builder = new RemoveRangeBuilder<TEntity>();
            builderAction(builder);
            return await RemoveRangeAsync(builder, cancellationToken);
        }

        /// <inheritdoc/>
        public int RemoveRange(RemoveRangeBuilder<TEntity> builder)
        {
            try
            {
                if (builder.IsExeсutable)
                {
                    var query = SetQueryForRemoveExecution(builder);
                    return query.ExecuteDelete();
                }
                else
                {
                    var entities = GetEntitiesToRemove(builder);

                    _context.RemoveRange(entities);

                    if (builder.SaveChanges)
                        _context.SaveChanges();

                    return entities.Count();
                }
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to delete a range of entities", ex);
            }
        }

        /// <inheritdoc/>
        public int RemoveRange(Action<RemoveRangeBuilder<TEntity>> builderAction)
        {
            var builder = new RemoveRangeBuilder<TEntity>();
            builderAction(builder);
            return RemoveRange(builder);
        }

        private TEntity? GetEntityToRemove(RemoveSingleBuilder<TEntity> builder)
        {
            return builder.RemoveStrategy switch
            {
                EntityRemoveStrategy.Entity when builder.Entity is not null => builder.Entity,
                EntityRemoveStrategy.Identifier when builder.Id is not null => _dbSet.Find(builder.Id),
                EntityRemoveStrategy.Filter when builder.Filter is not null => _dbSet.FirstOrDefault(builder.Filter),
                _ => throw new EntityException($"Invalid {nameof(builder.RemoveStrategy)}")
            };
        }

        private async Task<TEntity?> GetEntityToRemoveAsync(RemoveSingleBuilder<TEntity> builder, CancellationToken cancellationToken)
        {
            return builder.RemoveStrategy switch
            {
                EntityRemoveStrategy.Entity when builder.Entity is not null => builder.Entity,
                EntityRemoveStrategy.Identifier when builder.Id is not null => await _dbSet.FindAsync([builder.Id, cancellationToken], cancellationToken: cancellationToken),
                EntityRemoveStrategy.Filter when builder.Filter is not null => await _dbSet.FirstOrDefaultAsync(builder.Filter, cancellationToken),
                _ => throw new EntityException($"Invalid {nameof(builder.RemoveStrategy)}")
            };
        }

        private IEnumerable<TEntity> GetEntitiesToRemove(RemoveRangeBuilder<TEntity> builder)
        {
            return builder.RemoveStrategy switch
            {
                EntityRemoveStrategy.Entity => builder.Entities,
                EntityRemoveStrategy.Identifier => _dbSet.Where(e => builder.Identifiers.Contains(e.Id)).ToArray(),
                EntityRemoveStrategy.Filter when builder.Filter is not null => _dbSet.Where(builder.Filter).ToArray(),
                _ => throw new EntityException($"Invalid {nameof(builder.RemoveStrategy)}")
            };
        }

        private async Task<IEnumerable<TEntity>> GetEntitiesToRemoveAsync(RemoveRangeBuilder<TEntity> builder, CancellationToken cancellationToken)
        {
            return builder.RemoveStrategy switch
            {
                EntityRemoveStrategy.Entity => builder.Entities,
                EntityRemoveStrategy.Identifier => await _dbSet.Where(e => builder.Identifiers.Contains(e.Id)).ToArrayAsync(cancellationToken),
                EntityRemoveStrategy.Filter when builder.Filter is not null => await _dbSet.Where(builder.Filter).ToArrayAsync(cancellationToken),
                _ => throw new EntityException($"Invalid {nameof(builder.RemoveStrategy)}")
            };
        }

        private IQueryable<TEntity> SetQueryForRemoveExecution(RemoveSingleBuilder<TEntity> builder)
        {
            return builder.RemoveStrategy switch
            {
                EntityRemoveStrategy.Entity when builder.Entity is not null => _dbSet.Where(x => x.Id == builder.Entity.Id),
                EntityRemoveStrategy.Identifier when builder.Id is not null => _dbSet.Where(x => x.Id == builder.Id),
                EntityRemoveStrategy.Filter when builder.Filter is not null => _dbSet.Where(builder.Filter),
                _ => throw new EntityException($"Invalid {nameof(builder.RemoveStrategy)}")
            };
        }

        private IQueryable<TEntity> SetQueryForRemoveExecution(RemoveRangeBuilder<TEntity> builder)
        {
            return builder.RemoveStrategy switch
            {
                EntityRemoveStrategy.Entity => _dbSet.Where(x => builder.Entities.Select(x => x.Id).Contains(x.Id)),
                EntityRemoveStrategy.Identifier => _dbSet.Where(x => builder.Identifiers.Contains(x.Id)),
                EntityRemoveStrategy.Filter when builder.Filter is not null => _dbSet.Where(builder.Filter),
                _ => throw new EntityException($"Invalid {nameof(builder.RemoveStrategy)}")
            };
        }
    }

    public partial class Repository<TEntity, TDbContext>
    {
        /// <inheritdoc/>
        public async Task<int> UpdateAsync(UpdateSingleBuilder<TEntity> builder, CancellationToken cancellationToken = default)
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

                return 1;
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

        /// <inheritdoc/>
        public async Task<int> UpdateAsync(Action<UpdateSingleBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default)
        {
            var builder = new UpdateSingleBuilder<TEntity>();
            builderAction(builder);
            return await UpdateAsync(builder, cancellationToken);
        }

        /// <inheritdoc/>
        public int Update(UpdateSingleBuilder<TEntity> builder)
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

                return 1;
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to update an entity", ex);
            }
        }

        /// <inheritdoc/>
        public int Update(Action<UpdateSingleBuilder<TEntity>> builderAction)
        {
            var builder = new UpdateSingleBuilder<TEntity>();
            builderAction(builder);
            return Update(builder);
        }

        /// <inheritdoc/>
        public async Task<int> UpdateRangeAsync(UpdateRangeBuilder<TEntity> builder, CancellationToken cancellationToken = default)
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

                return builder.Entities.Count;
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

        /// <inheritdoc/>
        public async Task<int> UpdateRangeAsync(Action<UpdateRangeBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default)
        {
            var builder = new UpdateRangeBuilder<TEntity>();
            builderAction(builder);
            return await UpdateRangeAsync(builder, cancellationToken);
        }

        /// <inheritdoc/>
        public int UpdateRange(UpdateRangeBuilder<TEntity> builder)
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

                return builder.Entities.Count;
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to update a range of entities", ex);
            }
        }

        /// <inheritdoc/>
        public int UpdateRange(Action<UpdateRangeBuilder<TEntity>> builderAction)
        {
            var builder = new UpdateRangeBuilder<TEntity>();
            builderAction(builder);
            return UpdateRange(builder);
        }
    }

    public partial class Repository<TEntity, TDbContext>
    {
        /// <inheritdoc/>
        public async Task<int> RestoreAsync(RestoreSingleBuilder<TEntity> builder, CancellationToken cancellationToken = default)
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

                return 1;
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

        /// <inheritdoc/>
        public async Task<int> RestoreAsync(Action<RestoreSingleBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default)
        {
            var builder = new RestoreSingleBuilder<TEntity>();
            builderAction(builder);
            return await RestoreAsync(builder, cancellationToken);
        }

        /// <inheritdoc/>
        public int Restore(RestoreSingleBuilder<TEntity> builder)
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

                return 1;
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to restore a soft-deleted entity", ex);
            }
        }

        /// <inheritdoc/>
        public int Restore(Action<RestoreSingleBuilder<TEntity>> builderAction)
        {
            var builder = new RestoreSingleBuilder<TEntity>();
            builderAction(builder);
            return Restore(builder);
        }

        /// <inheritdoc/>
        public async Task<int> RestoreRangeAsync(RestoreRangeBuilder<TEntity> builder, CancellationToken cancellationToken = default)
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

                return builder.Entities.Count;
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

        /// <inheritdoc/>
        public async Task<int> RestoreRangeAsync(Action<RestoreRangeBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default)
        {
            var builder = new RestoreRangeBuilder<TEntity>();
            builderAction(builder);
            return await RestoreRangeAsync(builder, cancellationToken);
        }

        /// <inheritdoc/>
        public int RestoreRange(RestoreRangeBuilder<TEntity> builder)
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

                return builder.Entities.Count;
            }
            catch (Exception ex)
            {
                throw new EntityException("An error occurred while attempting to restore a range of soft-deleted entities", ex);
            }
        }

        /// <inheritdoc/>
        public int RestoreRange(Action<RestoreRangeBuilder<TEntity>> builderAction)
        {
            var builder = new RestoreRangeBuilder<TEntity>();
            builderAction(builder);
            return RestoreRange(builder);
        }
    }
}
