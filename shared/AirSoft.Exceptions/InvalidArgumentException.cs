using AirSoft.Exceptions.Base;
using System;

namespace AirSoft.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an invalid argument is provided.
    /// </summary>
    /// <remarks>
    /// This exception is used to handle errors where a method or constructor receives an argument
    /// that is invalid or does not meet the expected criteria. It provides utility methods for
    /// conditional exception throwing, ensuring that exceptions are thrown only when necessary.
    /// </remarks>
    public class InvalidArgumentException : AirSoftException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidArgumentException"/> class.
        /// </summary>
        public InvalidArgumentException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidArgumentException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public InvalidArgumentException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidArgumentException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public InvalidArgumentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
