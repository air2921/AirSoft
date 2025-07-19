using AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions.State.Remove;
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
        BaseEntityStateBuilder<RemoveSingleBuilder<TEntity>, TEntity>, IRemoveSingleBuilder<TEntity> where TEntity : IEntityBase
    {
        /// <summary>
        /// Identifier of the entity to be removed.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public object? Id { get; private set; }

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
        /// Specifies the removal mode (by object, identifiers or filter).
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public EntityRemoveMode RemoveMode { get; private set; }

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
        public RemoveSingleBuilder<TEntity> WithEntity(TEntity entity)
        {
            Entity = entity ?? throw new InvalidArgumentException("Entity for remove cannot be null");
            RemoveMode = EntityRemoveMode.Entity;
            return this;
        }

        /// <summary>
        /// Sets the identifier of entity to be removed.
        /// </summary>
        /// <param name="id">Entity identifier.</param>
        /// <returns>The current builder instance.</returns>
        public RemoveSingleBuilder<TEntity> WithIdentifier(object id)
        {
            Id = id ?? throw new InvalidArgumentException("Identifier of entity for remove cannot be null");
            RemoveMode = EntityRemoveMode.Identifier;
            return this;
        }

        /// <summary>
        /// Sets the filter expression to select single entity for removal.
        /// </summary>
        /// <param name="filter">Filter expression to select entity.</param>
        /// <returns>The current builder instance.</returns>
        public RemoveSingleBuilder<TEntity> WithFilter(Expression<Func<TEntity, bool>> filter)
        {
            Filter = filter ?? throw new InvalidArgumentException("Filter for filtering entity cannot be null");
            RemoveMode = EntityRemoveMode.Filter;
            return this;
        }

        /// <summary>
        /// Explicitly sets the removal mode.
        /// </summary>
        /// <param name="mode">The removal mode to use.</param>
        /// <returns>The current builder instance.</returns>
        public RemoveSingleBuilder<TEntity> WithRemoveMode(EntityRemoveMode mode)
        {
            RemoveMode = mode;
            return this;
        }
    }
}
