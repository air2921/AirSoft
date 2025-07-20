using System;

namespace AirSoft.Exceptions.Base
{
    /// <summary>
    /// Represents an exception that is thrown when an error occurs during telecom operations (SMS, calls, etc.).
    /// </summary>
    /// <remarks>
    /// This exception is used to handle telecom-specific and smtp errors like message delivery failures,
    /// call setup issues, or server/provider problems.
    /// </remarks>
    public abstract class SenderException : AirSoftException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SenderException"/> class.
        /// </summary>
        public SenderException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SenderException/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public SenderException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SenderException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public SenderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
