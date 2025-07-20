using AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions.Query;
using AirSoft.EntityFrameworkCore.Abstractions.Details;
using AirSoft.Exceptions;
using System.Linq.Expressions;

namespace AirSoft.EntityFrameworkCore.Abstractions.Repository
{
    public interface IGetRepository<TEntity> where TEntity : IEntityBase
    {
        /// <summary>
        /// Asynchronously retrieves the count of entities that match the specified filter.
        /// </summary>
        /// <param name="filter">Optional filter expression</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the count of matching entities.</returns>
        public Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? filter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the count of entities that match the specified filter.
        /// </summary>
        /// <param name="filter">Optional filter expression</param>
        /// <returns>The count of matching entities.</returns>
        public int GetCount(Expression<Func<TEntity, bool>>? filter);

        /// <summary>
        /// Asynchronously retrieves an entity by its identifier.
        /// </summary>
        /// <param name="id">Entity identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the found entity or null.</returns>
        public Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves an entity by its identifier.
        /// </summary>
        /// <param name="id">Entity identifier</param>
        /// <returns>The found entity or null.</returns>
        public TEntity? GetById(object id);

        /// <summary>
        /// Asynchronously retrieves an entity using a configured query builder.
        /// </summary>
        /// <param name="builder">Configured query builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the found entity or null.</returns>
        public Task<TEntity?> GetSingleAsync(ISingleQueryBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves an entity by configuring the query builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the query builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the found entity or null.</returns>
        public Task<TEntity?> GetSingleAsync(Action<ISingleQueryBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves an entity using a configured query builder.
        /// </summary>
        /// <param name="builder">Configured query builder</param>
        /// <returns>The found entity or null.</returns>
        public TEntity? GetSingle(ISingleQueryBuilder<TEntity> builder);

        /// <summary>
        /// Retrieves an entity by configuring the query builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the query builder</param>
        /// <returns>The found entity or null.</returns>
        public TEntity? GetSingle(Action<ISingleQueryBuilder<TEntity>> builderAction);

        /// <summary>
        /// Asynchronously retrieves multiple entities using a configured query builder.
        /// </summary>
        /// <param name="builder">Configured query builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns a collection of entities.</returns>
        public Task<IEnumerable<TEntity>> GetRangeAsync(IRangeQueryBuilder<TEntity>? builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves multiple entities by configuring the query builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the query builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns a collection of entities.</returns>
        public Task<IEnumerable<TEntity>> GetRangeAsync(Action<IRangeQueryBuilder<TEntity>>? builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves multiple entities using a configured query builder.
        /// </summary>
        /// <param name="builder">Configured query builder</param>
        /// <returns>A collection of entities.</returns>
        public IEnumerable<TEntity> GetRange(IRangeQueryBuilder<TEntity>? builder);

        /// <summary>
        /// Retrieves multiple entities by configuring the query builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the query builder</param>
        /// <returns>A collection of entities.</returns>
        public IEnumerable<TEntity> GetRange(Action<IRangeQueryBuilder<TEntity>>? builderAction);

        /// <summary>
        /// Asynchronously retrieves a range of entities with total count.
        /// </summary>
        /// <param name="builder">Configured query builder</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A <see cref="EntityChunkDetails{TEntity}"/> chunk of entities with total count.</returns>
        public Task<EntityChunkDetails<TEntity>> GetRangeEntireAsync(IRangeQueryBuilder<TEntity>? builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves a range of entities with total count by configuring the query builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the query builder</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A <see cref="EntityChunkDetails{TEntity}"/> chunk of entities with total count.</returns>
        public Task<EntityChunkDetails<TEntity>> GetRangeEntireAsync(Action<IRangeQueryBuilder<TEntity>>? builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a range of entities with total count.
        /// </summary>
        /// <param name="builder">Configured query builder</param>
        /// <returns>A <see cref="EntityChunkDetails{TEntity}"/> chunk of entities with total count.</returns>
        public EntityChunkDetails<TEntity> GetRangeEntire(IRangeQueryBuilder<TEntity>? builder);

        /// <summary>
        /// Retrieves a range of entities with total count by configuring the query builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the query builder</param>
        /// <returns>A <see cref="EntityChunkDetails{TEntity}"/> chunk of entities with total count.</returns>
        public EntityChunkDetails<TEntity> GetRangeEntire(Action<IRangeQueryBuilder<TEntity>>? builderAction);
    }
}
