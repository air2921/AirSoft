using AirSoft.EntityFrameworkCore.Abstractions.Entities;
using System.ComponentModel;

namespace AirSoft.EntityFrameworkCore.Abstractions.Builders.Base
{
    /// <summary>
    /// Abstract base class for command builders that perform database operations without returning query results.
    /// Serves as the foundation for create, update, and delete operation builders.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the derived builder (for fluent chaining).</typeparam>
    /// <typeparam name="TEntity">The type of the entity being built for.</typeparam>
    public abstract class BaseEntityStateBuilder<TBuilder, TEntity> :
        BaseEntityBuilder<TBuilder, TEntity>
        where TBuilder : BaseEntityStateBuilder<TBuilder, TEntity>
        where TEntity : EntityBase
    {
        /// <summary>
        /// Gets whether changes should be immediately persisted to the database.
        /// When true, changes are saved immediately; when false, changes are tracked but not persisted until explicitly saved.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool SaveChanges { get; private set; } = false;

        /// <summary>
        /// Configures whether the operation should persist changes to the database immediately.
        /// </summary>
        /// <param name="save">
        /// If true, changes will be saved immediately (default behavior for most operations).
        /// If false, changes will be tracked but not persisted until an explicit save is performed.
        /// </param>
        /// <returns>The current builder instance for fluent chaining.</returns>
        public TBuilder WithSaveChanges(bool save = true)
        {
            SaveChanges = save;
            return (TBuilder)this;
        }
    }
}
