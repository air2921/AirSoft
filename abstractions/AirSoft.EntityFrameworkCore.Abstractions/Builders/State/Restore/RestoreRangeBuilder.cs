using AirSoft.EntityFrameworkCore.Abstractions;
using AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions.State.Restore;
using AirSoft.EntityFrameworkCore.Abstractions.Builders.Base;
using System.ComponentModel;

namespace AirSoft.EntityFrameworkCore.Abstractions.Builders.State.Restore
{
    /// <summary>
    /// Fluent builder for restoring a collection of soft-deleted entities.
    /// Enables bulk restoration of entities that implement soft delete functionality.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to restore, must inherit from IEntityBase and implement soft delete.</typeparam>
    public sealed class RestoreRangeBuilder<TEntity> :
        BaseEntityStateBuilder<RestoreRangeBuilder<TEntity>, TEntity>, IRestoreRangeBuilder<TEntity> where TEntity : IEntityBase
    {
        /// <summary>
        /// Gets the collection of entities to be restored.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IReadOnlyCollection<TEntity> Entities { get; private set; } = [];

        /// <summary>
        /// Creates a new instance of the RestoreRangeBuilder.
        /// </summary>
        /// <returns>A new instance of RestoreRangeBuilder.</returns>
        public static RestoreRangeBuilder<TEntity> Create() => new();

        /// <summary>
        /// Sets the entities to be restored from soft-deleted state.
        /// </summary>
        /// <param name="entities">The collection of entities to restore.</param>
        /// <returns>The current builder instance.</returns>
        public RestoreRangeBuilder<TEntity> WithEntities(IEnumerable<TEntity> entities)
        {
            Entities = entities.ToArray();
            return this;
        }
    }
}
