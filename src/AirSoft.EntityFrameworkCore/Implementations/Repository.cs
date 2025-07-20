using AirSoft.EntityFrameworkCore.Abstractions;
using AirSoft.EntityFrameworkCore.Abstractions.Details;
using AirSoft.EntityFrameworkCore.Extensions;
using AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions.Query;
using AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions.State.Create;
using AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions.State.Remove;
using AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions.State.Restore;
using AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions.State.Update;
using AirSoft.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using AirSoft.EntityFrameworkCore.Abstractions.Entities;
using AirSoft.EntityFrameworkCore.Abstractions.Enums;

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
    /// It uses asynchronous operations to ensure non-blocking behavior and supports cancellation through <see cref="CancellationToken"/>.
    /// </remarks>
    public class Repository<TEntity, TDbContext> :
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
        /// Asynchronously retrieves the first or last entity that matches the specified filter, including options for ordering and tracking.
        /// </summary>
        /// <param name="builder">A <see cref="ISingleQueryBuilder{TEntity}"/> that defines the query criteria.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>The first or last entity that matches the filter, or <c>null</c> if not found.</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during the retrieval operation.</exception>
        public async Task<TEntity?> GetSingleAsync(ISingleQueryBuilder<TEntity> builder, CancellationToken cancellationToken = default)
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
        /// Asynchronously retrieves a range of entities based on the specified query builder.
        /// </summary>
        /// <param name="builder">A <see cref="IRangeQueryBuilder{TEntity}"/> that defines the query criteria.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>A list of entities matching the query criteria.</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during the retrieval operation.</exception>
        public async Task<IEnumerable<TEntity>> GetRangeAsync(IRangeQueryBuilder<TEntity>? builder, CancellationToken cancellationToken = default)
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
        /// Asynchronously retrieves a range of entities with total count.
        /// </summary>
        /// <param name="builder">A <see cref="IRangeQueryBuilder{TEntity}"/> that defines the query criteria.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A <see cref="EntityChunkDetails{TEntity}"/> chunk of entities with total count.</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during the retrieval operation.</exception>
        public async Task<EntityChunkDetails<TEntity>> GetRangeEntireAsync(IRangeQueryBuilder<TEntity>? builder, CancellationToken cancellationToken = default)
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
        /// Asynchronously adds a new entity to the repository.
        /// </summary>
        /// <param name="builder">Preconfigured builder containing the entity to add.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>The added entity.</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during the add operation.</exception>
        public async Task<TEntity> AddAsync(ICreateSingleBuilder<TEntity> builder, CancellationToken cancellationToken = default)
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
        /// Asynchronously adds multiple entities to the repository using a configured builder.
        /// </summary>
        /// <param name="builder">Preconfigured builder containing the entities to add.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>The collection of added entities.</returns>
        /// <exception cref="EntityException">
        /// Thrown when an error occurs during the add operation or when operation is canceled.
        /// </exception>
        public async Task<IEnumerable<TEntity>> AddRangeAsync(ICreateRangeBuilder<TEntity> builder, CancellationToken cancellationToken = default)
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
        /// Asynchronously deletes an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Builder with entity deletion parameters</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed</param>
        /// <returns>The deleted entity, or null if not found</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during deletion</exception>
        public async Task<TEntity?> DeleteAsync(IRemoveSingleBuilder<TEntity> builder, CancellationToken cancellationToken = default)
        {
            try
            {
                var timeout = builder.Timeout == TimeSpan.Zero ? TimeSpan.FromSeconds(RemoveTimeout) : builder.Timeout;
                using var cts = new CancellationTokenSource(timeout);
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                TEntity? entity = default;

                if (builder.RemoveMode == EntityRemoveMode.Entity && builder.Entity is not null)
                    entity = builder.Entity;

                if (builder.RemoveMode == EntityRemoveMode.Identifier && builder.Id is not null)
                    entity = await _dbSet.FindAsync([builder.Id, cancellationToken], cancellationToken: cancellationToken);

                if (builder.RemoveMode == EntityRemoveMode.Filter && builder.Filter is not null)
                    entity = await _dbSet.FirstOrDefaultAsync(builder.Filter, cancellationToken);

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
        /// Asynchronously deletes multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured builder with deletion parameters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Deleted entities</returns>
        /// <exception cref="EntityException">On deletion error</exception>
        public async Task<IEnumerable<TEntity>> DeleteRangeAsync(IRemoveRangeBuilder<TEntity> builder, CancellationToken cancellationToken = default)
        {
            try
            {
                var timeout = builder.Timeout == TimeSpan.Zero ? TimeSpan.FromSeconds(RemoveRangeTimeout) : builder.Timeout;
                using var cts = new CancellationTokenSource(timeout);
                using var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                cancellationToken = linkedToken.Token;

                if (builder.RemoveMode == EntityRemoveMode.Entity)
                {
                    _context.RemoveRange(builder.Entities);

                    if (builder.SaveChanges)
                        await _context.SaveChangesAsync(cancellationToken);

                    return builder.Entities;
                }

                if (builder.RemoveMode == EntityRemoveMode.Identifier)
                {
                    var entities = await _dbSet
                        .Where(e => builder.Identifiers.Contains(e.Id))
                        .ToArrayAsync(cancellationToken);

                    _context.RemoveRange(entities);

                    if (builder.SaveChanges)
                        await _context.SaveChangesAsync(cancellationToken);

                    return entities;
                }

                if (builder.RemoveMode == EntityRemoveMode.Filter && builder.Filter is not null)
                {
                    var entities = await _dbSet
                        .Where(builder.Filter)
                        .ToArrayAsync(cancellationToken);

                    _context.RemoveRange(entities);

                    if (builder.SaveChanges)
                        await _context.SaveChangesAsync(cancellationToken);

                    return entities;
                }

                throw new EntityException($"Invalid {nameof(builder.RemoveMode)}");
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
        /// Asynchronously updates an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Builder with entity update parameters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The updated entity</returns>
        /// <exception cref="EntityException">On update error</exception>
        public async Task<TEntity> UpdateAsync(IUpdateSingleBuilder<TEntity> builder, CancellationToken cancellationToken = default)
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
        /// Asynchronously updates multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured builder with update parameters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated entities</returns>
        /// <exception cref="EntityException">On update error</exception>
        public async Task<IEnumerable<TEntity>> UpdateRangeAsync(IUpdateRangeBuilder<TEntity> builder, CancellationToken cancellationToken = default)
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
        /// Asynchronously restores a soft-deleted entity using a configured builder.
        /// </summary>
        /// <param name="builder">Builder with entity restore parameters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The restored entity</returns>
        /// <exception cref="EntityException">On restore error</exception>
        public async Task<TEntity> RestoreAsync(IRestoreSingleBuilder<TEntity> builder, CancellationToken cancellationToken = default)
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
        /// Asynchronously restores multiple soft-deleted entities using a configured builder.
        /// </summary>
        /// <param name="builder">Builder with entities to restore</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Restored entities</returns>
        /// <exception cref="EntityException">On restore error</exception>
        public async Task<IEnumerable<TEntity>> RestoreRangeAsync(IRestoreRangeBuilder<TEntity> builder, CancellationToken cancellationToken = default)
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
    }
}
