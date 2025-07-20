using AirSoft.EntityFrameworkCore.Abstractions.Builders.State.Add;

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
        public Task<TEntity> AddAsync(AddSingleBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously adds an entity by configuring the builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the create builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the added entity.</returns>
        public Task<TEntity> AddAsync(Action<AddSingleBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Configured create builder</param>
        /// <returns>The added entity.</returns>
        public TEntity Add(AddSingleBuilder<TEntity> builder);

        /// <summary>
        /// Adds an entity by configuring the builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the create builder</param>
        /// <returns>The added entity.</returns>
        public TEntity Add(Action<AddSingleBuilder<TEntity>> builderAction);

        /// <summary>
        /// Asynchronously adds multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured create builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns a collection of added entities.</returns>
        public Task<IEnumerable<TEntity>> AddRangeAsync(AddRangeBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously adds multiple entities by configuring the builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the create builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns a collection of added entities.</returns>
        public Task<IEnumerable<TEntity>> AddRangeAsync(Action<AddRangeBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured create builder</param>
        /// <returns>A collection of added entities.</returns>
        public IEnumerable<TEntity> AddRange(AddRangeBuilder<TEntity> builder);

        /// <summary>
        /// Adds multiple entities by configuring the builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the create builder</param>
        /// <returns>A collection of added entities.</returns>
        public IEnumerable<TEntity> AddRange(Action<AddRangeBuilder<TEntity>> builderAction);
    }
}
