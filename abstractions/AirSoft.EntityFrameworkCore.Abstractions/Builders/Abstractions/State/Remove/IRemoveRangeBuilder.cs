using AirSoft.EntityFrameworkCore.Abstractions.Enums;
using System.ComponentModel;
using System.Linq.Expressions;

namespace AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions.State.Remove
{
    /// <summary>
    /// Defines a contract for building batch entity removal operations with support for multiple removal strategies
    /// </summary>
    /// <typeparam name="TEntity">The type of entities to remove, must implement <see cref="IEntityBase"/></typeparam>
    /// <remarks>
    /// Enables configuring entity removal through either:
    /// - Direct entity references
    /// - Identifier-based removal
    /// - Filter-based removal
    /// </remarks>
    public interface IRemoveRangeBuilder<TEntity> where TEntity : IEntityBase
    {
        /// <summary>
        /// Collection of entities to be removed directly.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IReadOnlyCollection<TEntity> Entities { get; }

        /// <summary>
        /// Collection of entity identifiers for entities to be removed.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IReadOnlyCollection<object> Identifiers { get; }

        /// <summary>
        /// Filter expression to select entities for removal.
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
