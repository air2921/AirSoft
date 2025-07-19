using AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions.State.Update;
using AirSoft.EntityFrameworkCore.Abstractions.Builders.Base;
using System.ComponentModel;

namespace AirSoft.EntityFrameworkCore.Abstractions.Builders.State.Update
{
    /// <summary>
    /// Fluent builder for configuring bulk entity updates with optional audit tracking
    /// </summary>
    /// <typeparam name="TEntity">Type of entity to update, must inherit from IEntityBase</typeparam>
    public sealed class UpdateRangeBuilder<TEntity> :
        BaseEntityStateBuilder<UpdateRangeBuilder<TEntity>, TEntity>, IUpdateRangeBuilder<TEntity> where TEntity : IEntityBase
    {
        /// <summary>
        /// Collection of entities to be updated
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IReadOnlyCollection<TEntity> Entities { get; private set; } = [];

        /// <summary>
        /// Identifier of the user who performed the update (for auditing)
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string? UpdatedByUser { get; private set; }

        /// <summary>
        /// Creates a new builder instance
        /// </summary>
        public static UpdateRangeBuilder<TEntity> Create() => new();

        /// <summary>
        /// Sets the entities to be updated
        /// </summary>
        /// <param name="entities">Collection of entities</param>
        /// <returns>The current builder instance.</returns>
        public UpdateRangeBuilder<TEntity> WithEntities(IEnumerable<TEntity> entities)
        {
            Entities = entities.ToArray();
            return this;
        }

        /// <summary>
        /// Sets the user who performed the update
        /// </summary>
        /// <param name="user">User identifier/name</param>
        /// <returns>The current builder instance.</returns>
        public UpdateRangeBuilder<TEntity> WithUpdatedBy(string? user)
        {
            UpdatedByUser = user;
            return this;
        }
    }
}

