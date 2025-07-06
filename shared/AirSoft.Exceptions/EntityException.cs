using AirSoft.Exceptions.Base;
using System;

namespace AirSoft.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an error occurs during database operations.
    /// </summary>
    /// <remarks>
    /// This exception is used to handle errors specific to database entity interactions,
    /// such as SQL query failures, constraint violations, connection issues,
    /// transaction problems, or ORM-specific errors. It provides a specialized
    /// exception type for database-related error handling scenarios.
    /// </remarks>
    public class EntityException : AirSoftException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityException"/> class.
        /// </summary>
        public EntityException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public EntityException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public EntityException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
