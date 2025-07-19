using System.ComponentModel;

namespace AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions.State.Update
{
    /// <summary>
    /// Defines a contract for building single entity update operations with audit tracking support
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to update, must implement <see cref="IEntityBase"/></typeparam>
    /// <remarks>
    /// Provides a fluent interface for configuring updates of a single entity while maintaining
    /// separation between operation construction and execution.
    /// </remarks>
    public interface IUpdateSingleBuilder<TEntity> : IBaseEntityBuilder, IBaseEntityStateBuilder where TEntity : IEntityBase
    {
        /// <summary>
        /// The entity to be updated.
        /// <para>This property contains the entity instance with its updated property values that need to be persisted.</para>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TEntity Entity { get; }

        /// <summary>
        /// The identifier or name of the user who performed the update (optional).
        /// <para>This property can be used for audit purposes to track who made changes to the entity.</para>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string? UpdatedByUser { get; }
    }
}
