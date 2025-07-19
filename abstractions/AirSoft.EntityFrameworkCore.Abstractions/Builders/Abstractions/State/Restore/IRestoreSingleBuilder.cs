using System.ComponentModel;

namespace AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions.State.Restore
{
    /// <summary>
    /// Defines a contract for building single entity restoration operations for soft-deleted entities
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to restore, must implement <see cref="IEntityBase"/></typeparam>
    /// <remarks>
    /// Provides a fluent interface for configuring restoration of a single soft-deleted entity
    /// while maintaining separation between operation construction and execution.
    /// </remarks>
    public interface IRestoreSingleBuilder<TEntity> where TEntity : IEntityBase
    {
        /// <summary>
        /// Gets the entity to be restored from soft-deleted state.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TEntity Entity { get; }
    }
}
