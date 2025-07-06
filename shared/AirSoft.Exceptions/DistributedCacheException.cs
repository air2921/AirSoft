using AirSoft.Exceptions.Base;
using System;

namespace AirSoft.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an error occurs in distributed cache operations.
    /// </summary>
    /// <remarks>
    /// This exception is used to handle errors specific to distributed caching, such as connection failures,
    /// serialization issues, or cache key conflicts. It provides utility methods for conditional exception throwing,
    /// ensuring that exceptions are thrown only when necessary and preventing redundant exception creation.
    /// </remarks>
    public class DistributedCacheException : AirSoftException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DistributedCacheException"/> class.
        /// </summary>
        public DistributedCacheException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DistributedCacheException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public DistributedCacheException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DistributedCacheException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public DistributedCacheException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
