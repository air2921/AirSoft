using AirSoft.EntityFrameworkCore.Abstractions.Builders.Base;
using AirSoft.Exceptions;
using System.ComponentModel;

namespace AirSoft.EntityFrameworkCore.Abstractions.Builders.State.Add
{
    /// <summary>
    /// Fluent builder for configuring single entity creation with optional audit tracking
    /// </summary>
    /// <typeparam name="TEntity">Type of entity to create, must inherit from IEntityBase</typeparam>
    public sealed class AddSingleBuilder<TEntity> :
        BaseEntityStateBuilder<AddSingleBuilder<TEntity>, TEntity> where TEntity : IEntityBase
    {
        /// <summary>
        /// Entity to be created
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TEntity Entity { get; private set; } = default!;

        /// <summary>
        /// Identifier of the user who performed the creation (for auditing)
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string? CreatedByUser { get; private set; }

        /// <summary>
        /// Creates a new builder instance
        /// </summary>
        /// <returns>New instance of CreateSingleBuilder</returns>
        public static AddSingleBuilder<TEntity> Create() => new();

        /// <summary>
        /// Sets the entity to be created
        /// </summary>
        /// <param name="entity">Entity instance</param>
        /// <returns>The current builder instance.</returns>
        /// exception cref="InvalidArgumentException">Thrown when entity is null</exception>
        public AddSingleBuilder<TEntity> WithEntity(TEntity entity)
        {
            Entity = entity ?? throw new InvalidArgumentException("Entity for creation cannot be null");
            return this;
        }

        /// <summary>
        /// Sets the user who performed the creation
        /// </summary>
        /// <param name="user">User identifier/name</param>
        /// <returns>The current builder instance.</returns>
        public AddSingleBuilder<TEntity> WithCreatedBy(string? user)
        {
            CreatedByUser = user;
            return this;
        }
    }
}
