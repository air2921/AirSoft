namespace AirSoft.EntityFrameworkCore.Abstractions.Enums
{
    /// <summary>
    /// Specifies the strategy for removing objects from a collection or repository.
    /// <b>Use carefully as different modes may have different performance implications.</b>
    /// </summary>
    public enum EntityRemoveStrategy
    {
        /// <summary>
        /// Remove object by unique identifier (ID = 101).
        /// <para>
        /// This mode is efficient for bulk operations where you only have the ID of object.
        /// Typically used when you want to avoid loading full objects into memory.
        /// </para>
        /// </summary>
        Identifier = 101,

        /// <summary>
        /// Remove actual entity (ID = 201).
        /// <para>
        /// This mode is useful when you already have loaded entity and want to remove it.
        /// </para>
        /// </summary>
        Entity = 201,

        /// <summary>
        /// Remove object by filter (ID = 301).
        /// <para>
        /// This mode is effective for bulk operations where you don't have an object or object ID.
        /// Typically used when you want to avoid loading full objects into memory.
        /// </para>
        /// </summary>
        Filter = 301
    }
}
