namespace AirSoft.EntityFrameworkCore.Abstractions
{
    public interface IEntityBase
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the entity was created, in UTC format.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the entity was last modified, in UTC format.
        /// </summary>
        public DateTimeOffset? UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who created the entity.
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who last modified the entity.
        /// </summary>
        public string? UpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is logically deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
