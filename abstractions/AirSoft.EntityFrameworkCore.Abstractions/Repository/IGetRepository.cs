using AirSoft.EntityFrameworkCore.Abstractions.Builders.Query;
using AirSoft.EntityFrameworkCore.Abstractions.Details;
using AirSoft.Exceptions;
using System.Linq.Expressions;

namespace AirSoft.EntityFrameworkCore.Abstractions.Repository
{
    /// <summary>
    /// Provides repository pattern operations for retrieving entities from the data store.
    /// Supports various query operations including single/multiple entity retrieval, counting, and paginated results.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to retrieve, must implement IEntityBase</typeparam>
    public interface IGetRepository<TEntity> where TEntity : IEntityBase
    {
        /// <summary>
        /// Asynchronously retrieves the count of entities that match the specified filter.
        /// </summary>
        /// <param name="filter">Optional filter expression</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the count of matching entities.</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? filter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the count of entities that match the specified filter.
        /// </summary>
        /// <param name="filter">Optional filter expression</param>
        /// <returns>The count of matching entities.</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public int GetCount(Expression<Func<TEntity, bool>>? filter);

        /// <summary>
        /// Asynchronously retrieves an entity by its identifier.
        /// </summary>
        /// <param name="id">Entity identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the found entity or null.</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves an entity by its identifier.
        /// </summary>
        /// <param name="id">Entity identifier</param>
        /// <returns>The found entity or null.</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public TEntity? GetById(object id);

        /// <summary>
        /// Asynchronously retrieves an entity using a configured query builder.
        /// </summary>
        /// <param name="builder">Configured query builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the found entity or null.</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<TEntity?> GetSingleAsync(SingleQueryBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves an entity by configuring the query builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the query builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the found entity or null.</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<TEntity?> GetSingleAsync(Action<SingleQueryBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves an entity using a configured query builder.
        /// </summary>
        /// <param name="builder">Configured query builder</param>
        /// <returns>The found entity or null.</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public TEntity? GetSingle(SingleQueryBuilder<TEntity> builder);

        /// <summary>
        /// Retrieves an entity by configuring the query builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the query builder</param>
        /// <returns>The found entity or null.</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public TEntity? GetSingle(Action<SingleQueryBuilder<TEntity>> builderAction);

        /// <summary>
        /// Asynchronously retrieves multiple entities using a configured query builder.
        /// </summary>
        /// <param name="builder">Configured query builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns a collection of entities.</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<IEnumerable<TEntity>> GetRangeAsync(RangeQueryBuilder<TEntity>? builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves multiple entities by configuring the query builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the query builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns a collection of entities.</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<IEnumerable<TEntity>> GetRangeAsync(Action<RangeQueryBuilder<TEntity>>? builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves multiple entities using a configured query builder.
        /// </summary>
        /// <param name="builder">Configured query builder</param>
        /// <returns>A collection of entities.</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public IEnumerable<TEntity> GetRange(RangeQueryBuilder<TEntity>? builder);

        /// <summary>
        /// Retrieves multiple entities by configuring the query builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the query builder</param>
        /// <returns>A collection of entities.</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public IEnumerable<TEntity> GetRange(Action<RangeQueryBuilder<TEntity>>? builderAction);

        /// <summary>
        /// Asynchronously retrieves a range of entities with total count.
        /// </summary>
        /// <param name="builder">Configured query builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A <see cref="EntityChunkDetails{TEntity}"/> chunk of entities with total count.</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<EntityChunkDetails<TEntity>> GetRangeEntireAsync(RangeQueryBuilder<TEntity>? builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves a range of entities with total count by configuring the query builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the query builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A <see cref="EntityChunkDetails{TEntity}"/> chunk of entities with total count.</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<EntityChunkDetails<TEntity>> GetRangeEntireAsync(Action<RangeQueryBuilder<TEntity>>? builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a range of entities with total count.
        /// </summary>
        /// <param name="builder">Configured query builder</param>
        /// <returns>A <see cref="EntityChunkDetails{TEntity}"/> chunk of entities with total count.</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public EntityChunkDetails<TEntity> GetRangeEntire(RangeQueryBuilder<TEntity>? builder);

        /// <summary>
        /// Retrieves a range of entities with total count by configuring the query builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the query builder</param>
        /// <returns>A <see cref="EntityChunkDetails{TEntity}"/> chunk of entities with total count.</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public EntityChunkDetails<TEntity> GetRangeEntire(Action<RangeQueryBuilder<TEntity>>? builderAction);
    }
}
