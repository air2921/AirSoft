using AirSoft.Exceptions.Base;
using System;

namespace AirSoft.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an error occurs during Amazon S3 client operations.
    /// </summary>
    /// <remarks>
    /// This exception is used to handle errors specific to Amazon S3, such as connection failures,
    /// bucket access issues, or file upload/download errors. It provides utility methods for conditional exception throwing.
    /// </remarks>
    public class S3ClientException : AirSoftException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="S3ClientException"/> class.
        /// </summary>
        public S3ClientException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="S3ClientException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public S3ClientException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="S3ClientException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public S3ClientException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
