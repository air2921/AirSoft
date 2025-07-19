using System.ComponentModel;

namespace AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions
{
    /// <summary>
    /// An interface for command builders that perform database operations without returning query results.
    /// Serves as the foundation for create, update, and delete operation builders.
    /// </summary>
    public interface IBaseEntityStateBuilder
    {
        /// <summary>
        /// Gets whether changes should be immediately persisted to the database.
        /// When true, changes are saved immediately; when false, changes are tracked but not persisted until explicitly saved.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool SaveChanges { get; }
    }
}
