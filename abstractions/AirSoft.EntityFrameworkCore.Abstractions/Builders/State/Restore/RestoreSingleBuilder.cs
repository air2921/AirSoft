using AirSoft.EntityFrameworkCore.Abstractions.Builders.Base;
using AirSoft.Exceptions;
using System.ComponentModel;

namespace AirSoft.EntityFrameworkCore.Abstractions.Builders.State.Restore
{
    /// <summary>
    /// Fluent builder for restoring a single soft-deleted entity.
    /// Provides a type-safe way to restore entities that implement soft delete functionality.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The type of entity to restore, must inherit from IEntityBase and implement soft delete.
    /// </typeparam>
    public sealed class RestoreSingleBuilder<TEntity> :
        BaseEntityStateBuilder<RestoreSingleBuilder<TEntity>, TEntity> where TEntity : IEntityBase
    {
        /// <summary>
        /// Gets the entity to be restored from soft-deleted state.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TEntity Entity { get; private set; } = default!;

        /// <summary>
        /// Creates a new instance of the RestoreSingleBuilder.
        /// </summary>
        /// <returns>A new instance of RestoreSingleBuilder.</returns>
        public static RestoreSingleBuilder<TEntity> Create() => new();

        /// <summary>
        /// Sets the entity to be restored from soft-deleted state.
        /// </summary>
        /// <param name="entity">The entity instance to restore.</param>
        /// <returns>The current builder instance.</returns>
        /// <exception cref="InvalidArgumentException">Thrown when entity is null</exception>
        public RestoreSingleBuilder<TEntity> WithEntity(TEntity entity)
        {
            Entity = entity ?? throw new InvalidArgumentException("Entity for restore cannot be null");
            return this;
        }
    }
}
