using AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions.State.Create;
using AirSoft.EntityFrameworkCore.Abstractions.Builders.Base;
using System.ComponentModel;

namespace AirSoft.EntityFrameworkCore.Abstractions.Builders.State.Create
{
    /// <summary>
    /// Fluent builder for configuring bulk entity creation with optional audit tracking
    /// </summary>
    /// <typeparam name="TEntity">Type of entity to create, must inherit from IEntityBase</typeparam>
    public sealed class CreateRangeBuilder<TEntity> :
        BaseEntityStateBuilder<CreateRangeBuilder<TEntity>, TEntity>, ICreateRangeBuilder<TEntity> where TEntity : IEntityBase
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
        public static CreateRangeBuilder<TEntity> Create() => new();

        /// <summary>
        /// Sets the entities to be created
        /// </summary>
        /// <param name="entities">Collection of entities</param>
        /// <returns>The current builder instance.</returns>
        public CreateRangeBuilder<TEntity> WithEntities(IEnumerable<TEntity> entities)
        {
            Entities = entities.ToArray();
            return this;
        }

        /// <summary>
        /// Sets the user who performed the creation
        /// </summary>
        /// <param name="user">User identifier/name</param>
        /// <returns>The current builder instance.</returns>
        public CreateRangeBuilder<TEntity> WithCreatedBy(string? user)
        {
            CreatedByUser = user;
            return this;
        }
    }
}
