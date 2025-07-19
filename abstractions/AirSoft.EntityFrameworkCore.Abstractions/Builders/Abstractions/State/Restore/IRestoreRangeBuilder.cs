using System.ComponentModel;

namespace AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions.State.Restore
{
    /// <summary>
    /// Defines a contract for building batch entity restoration operations for soft-deleted entities
    /// </summary>
    /// <typeparam name="TEntity">The type of entities to restore, must implement <see cref="IEntityBase"/></typeparam>
    /// <remarks>
    /// Provides a fluent interface for configuring restoration of multiple soft-deleted entities
    /// while maintaining separation between operation construction and execution.
    /// </remarks>
    public interface IRestoreRangeBuilder<TEntity> : IBaseEntityBuilder, IBaseEntityStateBuilder where TEntity : IEntityBase
    {
        /// <summary>
        /// Gets the collection of entities to be restored.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IReadOnlyCollection<TEntity> Entities { get; }
    }
}
