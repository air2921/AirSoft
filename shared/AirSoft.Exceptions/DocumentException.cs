using AirSoft.Exceptions.Base;
using System;

namespace AirSoft.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an error occurs during MongoDB database operations.
    /// </summary>
    /// <remarks>
    /// This exception is used to handle errors specific to MongoDB document interactions,
    /// such as query failures, document validation errors, connection issues,
    /// or write operation conflicts. It provides a specialized exception type
    /// for MongoDB-related error handling scenarios.
    /// </remarks>
    public class DocumentException : AirSoftException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentException"/> class.
        /// </summary>
        public DocumentException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public DocumentException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public DocumentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
