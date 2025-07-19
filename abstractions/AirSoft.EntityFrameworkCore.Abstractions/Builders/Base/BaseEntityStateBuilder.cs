using AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions;
using System.ComponentModel;

namespace AirSoft.EntityFrameworkCore.Abstractions.Builders.Base
{
    /// <summary>
    /// Abstract base class for command builders that perform database operations without returning query results.
    /// Serves as the foundation for create, update, and delete operation builders.
    /// </summary>
    public abstract class BaseEntityStateBuilder<TBuilder, TEntity> :
        BaseEntityBuilder<TBuilder, TEntity>, IBaseEntityStateBuilder
        where TBuilder : BaseEntityStateBuilder<TBuilder, TEntity>
        where TEntity : IEntityBase
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
