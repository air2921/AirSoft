using System.ComponentModel;

namespace AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions.State.Create
{
    /// <summary>
    /// Defines a contract for building single entity creation operations with audit tracking support
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to create, must implement <see cref="IEntityBase"/></typeparam>
    /// <remarks>
    /// Provides a fluent interface for configuring entity creation while maintaining
    /// separation between operation construction and execution.
    /// </remarks>
    public interface ICreateSingleBuilder<TEntity> : IBaseEntityBuilder, IBaseEntityStateBuilder where TEntity : IEntityBase
    {
        /// <summary>
        /// Entity to be created
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TEntity Entity { get; }

        /// <summary>
        /// Identifier of the user who performed the creation (for auditing)
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string? CreatedByUser { get; }
    }
}
