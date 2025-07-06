using AirSoft.EntityFrameworkCore.Builders.Base;
using AirSoft.EntityFrameworkCore.Entities;
using AirSoft.Exceptions;
using System.ComponentModel;

namespace AirSoft.EntityFrameworkCore.Builders.State.Create
{
    /// <summary>
    /// Fluent builder for configuring single entity creation with optional audit tracking
    /// </summary>
    /// <typeparam name="TEntity">Type of entity to create, must inherit from EntityBase</typeparam>
    public sealed class CreateSingleBuilder<TEntity> : BaseEntityStateBuilder<CreateSingleBuilder<TEntity>, TEntity> where TEntity : EntityBase
    {
        /// <summary>
        /// Entity to be created
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal TEntity Entity { get; private set; } = default!;

        /// <summary>
        /// Identifier of the user who performed the creation (for auditing)
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal string? CreatedByUser { get; private set; }

        /// <summary>
        /// Creates a new builder instance
        /// </summary>
        /// <returns>New instance of CreateSingleBuilder</returns>
        public static CreateSingleBuilder<TEntity> Create() => new();

        /// <summary>
        /// Sets the entity to be created
        /// </summary>
        /// <param name="entity">Entity instance</param>
        /// <returns>The current builder instance.</returns>
        public CreateSingleBuilder<TEntity> WithEntity(TEntity entity)
        {
            Entity = entity ?? throw new InvalidArgumentException("Entity for creation cannot be null");
            return this;
        }

        /// <summary>
        /// Sets the user who performed the creation
        /// </summary>
        /// <param name="user">User identifier/name</param>
        /// <returns>The current builder instance.</returns>
        public CreateSingleBuilder<TEntity> WithCreatedBy(string? user)
        {
            CreatedByUser = user;
            return this;
        }
    }
}
