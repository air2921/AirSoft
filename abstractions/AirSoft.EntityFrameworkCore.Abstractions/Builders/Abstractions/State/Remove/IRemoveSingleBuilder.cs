using AirSoft.EntityFrameworkCore.Abstractions.Enums;
using System.ComponentModel;
using System.Linq.Expressions;

namespace AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions.State.Remove
{
    /// <summary>
    /// Defines a contract for building single entity removal operations with multiple targeting options
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to remove, must implement <see cref="IEntityBase"/></typeparam>
    /// <remarks>
    /// Provides a flexible interface for configuring entity removal through:
    /// - Direct entity reference
    /// - Primary key identifier
    /// - Filter expression
    /// </remarks>
    public interface IRemoveSingleBuilder<TEntity> where TEntity : IEntityBase
    {
        /// <summary>
        /// Identifier of the entity to be removed.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public object? Id { get; }

        /// <summary>
        /// Entity instance to be removed.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TEntity? Entity { get; }

        /// <summary>
        /// Filter expression to select single entity for removal.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Expression<Func<TEntity, bool>>? Filter { get; }

        /// <summary>
        /// Specifies the removal mode (by object, identifiers or filter).
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public EntityRemoveMode RemoveMode { get; }
    }
}
