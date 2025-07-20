using AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions.State.Create;
using AirSoft.Exceptions;

namespace AirSoft.EntityFrameworkCore.Abstractions.Repository
{
    public interface IAddRepository<TEntity> where TEntity : IEntityBase
    {
        /// <summary>
        /// Asynchronously adds an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Configured create builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the added entity.</returns>
        public Task<TEntity> AddAsync(ICreateSingleBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously adds an entity by configuring the builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the create builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the added entity.</returns>
        public Task<TEntity> AddAsync(Action<ICreateSingleBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Configured create builder</param>
        /// <returns>The added entity.</returns>
        public TEntity Add(ICreateSingleBuilder<TEntity> builder);

        /// <summary>
        /// Adds an entity by configuring the builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the create builder</param>
        /// <returns>The added entity.</returns>
        public TEntity Add(Action<ICreateSingleBuilder<TEntity>> builderAction);

        /// <summary>
        /// Asynchronously adds multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured create builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns a collection of added entities.</returns>
        public Task<IEnumerable<TEntity>> AddRangeAsync(ICreateRangeBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously adds multiple entities by configuring the builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the create builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns a collection of added entities.</returns>
        public Task<IEnumerable<TEntity>> AddRangeAsync(Action<ICreateRangeBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured create builder</param>
        /// <returns>A collection of added entities.</returns>
        public IEnumerable<TEntity> AddRange(ICreateRangeBuilder<TEntity> builder);

        /// <summary>
        /// Adds multiple entities by configuring the builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the create builder</param>
        /// <returns>A collection of added entities.</returns>
        public IEnumerable<TEntity> AddRange(Action<ICreateRangeBuilder<TEntity>> builderAction);
    }
}
