using AirSoft.EntityFrameworkCore.Builders.Base;
using AirSoft.EntityFrameworkCore.Entities;
using System.ComponentModel;

namespace AirSoft.EntityFrameworkCore.Builders.State.Update
{
    /// <summary>
    /// Fluent builder for configuring bulk entity updates with optional audit tracking
    /// </summary>
    /// <typeparam name="TEntity">Type of entity to update, must inherit from EntityBase</typeparam>
    public sealed class UpdateRangeBuilder<TEntity> : BaseEntityStateBuilder<UpdateRangeBuilder<TEntity>, TEntity> where TEntity : EntityBase
    {
        /// <summary>
        /// Collection of entities to be updated
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal IReadOnlyCollection<TEntity> Entities { get; private set; } = [];

        /// <summary>
        /// Identifier of the user who performed the update (for auditing)
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal string? UpdatedByUser { get; private set; }

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

