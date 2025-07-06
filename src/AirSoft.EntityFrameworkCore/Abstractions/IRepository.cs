using AirSoft.EntityFrameworkCore.Builders.Query;
using AirSoft.EntityFrameworkCore.Builders.State.Create;
using AirSoft.EntityFrameworkCore.Builders.State.Remove;
using AirSoft.EntityFrameworkCore.Builders.State.Restore;
using AirSoft.EntityFrameworkCore.Builders.State.Update;
using AirSoft.EntityFrameworkCore.Details;
using AirSoft.EntityFrameworkCore.Entities;
using Microsoft.EntityFrameworkCore;
using AirSoft.Exceptions;
using System.Linq.Expressions;

namespace AirSoft.EntityFrameworkCore.Abstractions
{
    /// <summary>
    /// Represents a generic repository pattern for performing CRUD operations on entities of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity. It must inherit from <see cref="EntityBase"/>.</typeparam>
    public interface IRepository<TEntity> where TEntity : EntityBase
    {
        /// <summary>
        /// Asynchronously checks existing record that match the specified filter.
        /// </summary>
        /// <param name="filter">The filter expression to apply to the entity set.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>The true if record is exits otherwise false.</returns>
        /// <exception cref="EntityException">Thrown when an error occurs during the check exists operation.</exception>
        public Task<bool> IsExistsAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves the count of entities that match the specified filter.
        /// </summary>
        /// <param name="filter">Optional filter expression</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the count of matching entities.</returns>
        public Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? filter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves an entity by its identifier.
        /// </summary>
        /// <param name="id">Entity identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the found entity or null.</returns>
        public Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves an entity using a configured query builder.
        /// </summary>
        /// <param name="builder">Configured query builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the found entity or null.</returns>
        public Task<TEntity?> GetSingleAsync(SingleQueryBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves multiple entities using a configured query builder.
        /// </summary>
        /// <param name="builder">Configured query builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns a collection of entities.</returns>
        public Task<IEnumerable<TEntity>> GetRangeAsync(RangeQueryBuilder<TEntity>? builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves a range of entities with total count.
        /// </summary>
        /// <param name="builder">Configured query builder</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A <see cref="EntityChunkDetails{TEntity}"/> chunk of entities with total count.</returns>
        public Task<EntityChunkDetails<TEntity>> GetRangeEntireAsync(RangeQueryBuilder<TEntity>? builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously adds an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Configured create builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the added entity.</returns>
        public Task<TEntity> AddAsync(CreateSingleBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously adds multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured create builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns a collection of added entities.</returns>
        public Task<IEnumerable<TEntity>> AddRangeAsync(CreateRangeBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously deletes an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Configured delete builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the deleted entity or null.</returns>
        public Task<TEntity?> DeleteAsync(RemoveSingleBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously deletes multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured delete builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns a collection of deleted entities.</returns>
        public Task<IEnumerable<TEntity>> DeleteRangeAsync(RemoveRangeBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously updates an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Configured update builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the updated entity.</returns>
        public Task<TEntity> UpdateAsync(UpdateSingleBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously updates multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured update builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns a collection of updated entities.</returns>
        public Task<IEnumerable<TEntity>> UpdateRangeAsync(UpdateRangeBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously restores an entity using a configured builder.
        /// </summary>
        /// <param name="builder">Configured restore builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns the restored entity.</returns>
        public Task<TEntity> RestoreAsync(RestoreSingleBuilder<TEntity> builder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously restores multiple entities using a configured builder.
        /// </summary>
        /// <param name="builder">Configured restore builder</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A task that represents the asynchronous operation and returns a collection of restored entities.</returns>
        public Task<IEnumerable<TEntity>> RestoreRangeAsync(RestoreRangeBuilder<TEntity> builder, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Represents a generic repository pattern with support for a specific <typeparamref name="TDbContext"/> 
    /// for performing CRUD operations on entities of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity. It must inherit from <see cref="EntityBase"/>.</typeparam>
    /// <typeparam name="TDbContext">The type of the database context. It must inherit from <see cref="DbContext"/>.</typeparam>
    public interface IRepository<TEntity, TDbContext> : IRepository<TEntity> where TEntity : EntityBase where TDbContext : DbContext;
}