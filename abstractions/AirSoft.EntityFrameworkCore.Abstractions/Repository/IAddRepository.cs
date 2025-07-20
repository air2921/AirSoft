using AirSoft.EntityFrameworkCore.Abstractions.Builders.State.Add;
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
        /// <returns>A task that represents the asynchronous operation and returns the number of added entities (1 if successful, 0 otherwise).</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<int> AddAsync(AddSingleBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously adds an entity by configuring the builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the create builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the number of added entities (1 if successful, 0 otherwise).</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<int> AddAsync(Action<AddSingleBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Configured create builder</param>
        /// <returns>The number of added entities (1 if successful, 0 otherwise).</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public int Add(AddSingleBuilder<TEntity> builder);

        /// <summary>
        /// Adds an entity by configuring the builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the create builder</param>
        /// <returns>The number of added entities (1 if successful, 0 otherwise).</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public int Add(Action<AddSingleBuilder<TEntity>> builderAction);

        /// <summary>
        /// Asynchronously adds multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured create builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the number of added entities.</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<int> AddRangeAsync(AddRangeBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously adds multiple entities by configuring the builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the create builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the number of added entities.</returns>
        /// <exception cref="EntityException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<int> AddRangeAsync(Action<AddRangeBuilder<TEntity>> builderAction, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured create builder</param>
        /// <returns>The number of added entities.</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public int AddRange(AddRangeBuilder<TEntity> builder);

        /// <summary>
        /// Adds multiple entities by configuring the builder through an action.
        /// </summary>
        /// <param name="builderAction">Action to configure the create builder</param>
        /// <returns>The number of added entities.</returns>
        /// <exception cref="EntityException">Thrown when database operation fails</exception>
        public int AddRange(Action<AddRangeBuilder<TEntity>> builderAction);
    }
}
