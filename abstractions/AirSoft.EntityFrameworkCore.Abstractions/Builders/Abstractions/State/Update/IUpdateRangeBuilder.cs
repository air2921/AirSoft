using System.ComponentModel;

namespace AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions.State.Update
{
    /// <summary>
    /// Defines a contract for building batch entity update operations with audit tracking support
    /// </summary>
    /// <typeparam name="TEntity">The type of entities to update, must implement <see cref="IEntityBase"/></typeparam>
    /// <remarks>
    /// Provides a fluent interface for configuring updates of multiple entities while maintaining
    /// separation between operation construction and execution.
    /// </remarks>
    public interface IUpdateRangeBuilder<TEntity> : IBaseEntityBuilder, IBaseEntityStateBuilder where TEntity : IEntityBase
    {
        /// <summary>
        /// Collection of entities to be updated
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IReadOnlyCollection<TEntity> Entities { get; }

        /// <summary>
        /// Identifier of the user who performed the update (for auditing)
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string? UpdatedByUser { get; }
    }
}
