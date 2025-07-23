using AirSoft.EntityFrameworkCore.Abstractions.Builders.Base;
using AirSoft.EntityFrameworkCore.Abstractions.Entities;
using System.ComponentModel;

namespace AirSoft.EntityFrameworkCore.Abstractions.Builders.State.Add
{
    /// <summary>
    /// Fluent builder for configuring bulk entity creation with optional audit tracking
    /// </summary>
    /// <typeparam name="TEntity">Type of entity to create, must inherit from <see cref="EntityBase"/></typeparam>
    public sealed class AddRangeBuilder<TEntity> :
        BaseEntityStateBuilder<AddRangeBuilder<TEntity>, TEntity> where TEntity : EntityBase
    {
        /// <summary>
        /// Collection of entities to be created
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IReadOnlyCollection<TEntity> Entities { get; private set; } = [];

        /// <summary>
        /// Identifier of the user who performed the creation (for auditing)
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string? CreatedByUser { get; private set; }

        /// <summary>
        /// Creates a new builder instance
        /// </summary>
        public static AddRangeBuilder<TEntity> Create() => new();

        /// <summary>
        /// Sets the entities to be created
        /// </summary>
        /// <param name="entities">Collection of entities</param>
        /// <returns>The current builder instance.</returns>
        public AddRangeBuilder<TEntity> WithEntities(IEnumerable<TEntity> entities)
        {
            Entities = [.. entities];
            return this;
        }

        /// <summary>
        /// Sets the user who performed the creation
        /// </summary>
        /// <param name="user">User identifier/name</param>
        /// <returns>The current builder instance.</returns>
        public AddRangeBuilder<TEntity> WithCreatedBy(string? user)
        {
            CreatedByUser = user;
            return this;
        }
    }
}
