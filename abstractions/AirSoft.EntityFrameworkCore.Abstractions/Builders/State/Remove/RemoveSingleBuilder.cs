using AirSoft.EntityFrameworkCore.Abstractions.Builders.Base;
using AirSoft.EntityFrameworkCore.Abstractions.Enums;
using AirSoft.Exceptions;
using System.ComponentModel;
using System.Linq.Expressions;

namespace AirSoft.EntityFrameworkCore.Abstractions.Builders.State.Remove
{
    /// <summary>
    /// A builder class for configuring parameters to remove a single entity.
    /// Provides multiple ways to specify entity for removal: by instance, identifier, or filter expression.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to remove, must inherit from IEntityBase.</typeparam>
    public sealed class RemoveSingleBuilder<TEntity> :
        BaseEntityStateBuilder<RemoveSingleBuilder<TEntity>, TEntity> where TEntity : IEntityBase
    {
        /// <summary>
        /// Identifier of the entity to be removed.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string? Id { get; private set; }

        /// <summary>
        /// Entity instance to be removed.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TEntity? Entity { get; private set; }

        /// <summary>
        /// Filter expression to select single entity for removal.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Expression<Func<TEntity, bool>>? Filter { get; private set; }

        /// <summary>
        /// Specifies the removal strategy (by object, identifiers or filter).
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public EntityRemoveStrategy RemoveStrategy { get; private set; }

        /// <summary>
        /// Specifies whether entities should be deleted without the change tracker.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsExeсutable { get; private set; } = false;

        /// <summary>
        /// Creates a new instance of RemoveSingleBuilder.
        /// </summary>
        /// <returns>New instance of RemoveSingleBuilder</returns>
        public static RemoveSingleBuilder<TEntity> Create() => new();

        /// <summary>
        /// Sets the entity instance to be removed.
        /// </summary>
        /// <param name="entity">Entity instance to remove.</param>
        /// <returns>The current builder instance.</returns>
        /// <exception cref="InvalidArgumentException">Thrown when entity is null</exception>
        public RemoveSingleBuilder<TEntity> WithEntity(TEntity entity)
        {
            Entity = entity ?? throw new InvalidArgumentException("Entity for remove cannot be null");
            RemoveStrategy = EntityRemoveStrategy.Entity;
            return this;
        }

        /// <summary>
        /// Sets the identifier of entity to be removed.
        /// </summary>
        /// <param name="id">Entity identifier.</param>
        /// <returns>The current builder instance.</returns>
        /// <exception cref="InvalidArgumentException">Thrown when identifier is null</exception>
        public RemoveSingleBuilder<TEntity> WithIdentifier(string id)
        {
            Id = id ?? throw new InvalidArgumentException("Identifier of entity for remove cannot be null");
            RemoveStrategy = EntityRemoveStrategy.Identifier;
            return this;
        }

        /// <summary>
        /// Sets the filter expression to select single entity for removal.
        /// </summary>
        /// <param name="filter">Filter expression to select entity.</param>
        /// <returns>The current builder instance.</returns>
        /// <exception cref="InvalidArgumentException">Thrown when filter is null</exception>
        public RemoveSingleBuilder<TEntity> WithFilter(Expression<Func<TEntity, bool>> filter)
        {
            Filter = filter ?? throw new InvalidArgumentException("Filter for filtering entity cannot be null");
            RemoveStrategy = EntityRemoveStrategy.Filter;
            return this;
        }

        /// <summary>
        /// Explicitly sets the removal strategy.
        /// </summary>
        /// <param name="strategy">The removal strategy to use.</param>
        /// <returns>The current builder instance.</returns>
        public RemoveSingleBuilder<TEntity> WithRemoveStrategy(EntityRemoveStrategy strategy)
        {
            RemoveStrategy = strategy;
            return this;
        }

        /// <summary>
        /// Configures whether to perform direct database deletion (bypassing change tracking and soft delete).
        /// </summary>
        /// <remarks>
        /// WARNING: When enabled, deletes records directly in database, ignoring:
        /// - Change tracking
        /// - Soft delete markers
        /// - Delete interceptors
        /// - Calling <see cref="BaseEntityStateBuilder{TRestoreRangeBuilder, TEntity}.WithSaveChanges(bool)"/> is not required (changes are applied immediately)
        /// This operation is irreversible.
        /// </remarks>
        /// <param name="executable">True for direct DB deletion, false for normal EF deletion</param>
        /// <returns>The current builder instance</returns>
        public RemoveSingleBuilder<TEntity> WithExecution(bool executable = true)
        {
            IsExeсutable = executable;
            return this;
        }
    }
}
