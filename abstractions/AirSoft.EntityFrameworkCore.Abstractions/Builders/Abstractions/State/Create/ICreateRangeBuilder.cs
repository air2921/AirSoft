using System.ComponentModel;

namespace AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions.State.Create
{
    /// <summary>
    /// Defines a contract for building bulk entity creation operations with optional audit tracking
    /// </summary>
    /// <typeparam name="TEntity">The type of entities to create, must implement <see cref="IEntityBase"/></typeparam>
    /// <remarks>
    /// Provides a fluent interface for configuring batch creation of entities while maintaining
    /// separation between operation construction and execution.
    /// </remarks>
    public interface ICreateRangeBuilder<TEntity> where TEntity : IEntityBase
    {
        /// <summary>
        /// Collection of entities to be created
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IReadOnlyCollection<TEntity> Entities { get; }

        /// <summary>
        /// Identifier of the user who performed the creation (for auditing)
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string? CreatedByUser { get; }
    }
}
