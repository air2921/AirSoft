using AirSoft.EntityFrameworkCore.Abstractions.Entities;

namespace AirSoft.EntityFrameworkCore.Abstractions.Details
{
    /// <summary>
    /// Represents a data chunk (subset) with total record count.
    /// Used for batch processing and pagination scenarios with entity collections.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity. It must inherit from <see cref="EntityBase"/>.</typeparam>
    /// <remarks>
    /// Combines a slice of data with the total available count, enabling operations like
    /// "showing 20 of 1000 records" while processing in manageable chunks.
    /// </remarks>
    public class EntityChunkDetails<TEntity> where TEntity : EntityBase
    {
        /// <summary>
        /// Gets or sets the current data chunk (subset of records).
        /// Typically represents one page or batch of entities.
        /// </summary>
        public IEnumerable<TEntity> Chunk { get; set; } = [];

        /// <summary>
        /// Gets or sets the total count of available records.
        /// Represents the full dataset size regardless of chunk size.
        /// </summary>
        public long Total { get; set; }
    }
}
