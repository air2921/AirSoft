namespace AirSoft.EntityFrameworkCore.Abstractions.Details
{
    /// <summary>
    /// Represents a data chunk (subset) with total record count.
    /// Used for batch processing and pagination scenarios with entity collections.
    /// </summary>
    /// <typeparam name="TValue">The object type, must be class.</typeparam>
    /// <remarks>
    /// Combines a slice of data with the total available count, enabling operations like
    /// "showing 20 of 1000 records" while processing in manageable chunks.
    /// </remarks>
    public class EntityChunkDetails<TValue> where TValue : IEntityBase
    {
        /// <summary>
        /// Gets or sets the current data chunk (subset of records).
        /// Typically represents one page or batch of entities.
        /// </summary>
        public IEnumerable<TValue> Chunk { get; set; } = [];

        /// <summary>
        /// Gets or sets the total count of available records.
        /// Represents the full dataset size regardless of chunk size.
        /// </summary>
        public long Total { get; set; }
    }
}
