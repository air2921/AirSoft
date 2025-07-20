using AirSoft.Exceptions.Base;
using System;

namespace AirSoft.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an error occurs during SMTP client operations.
    /// </summary>
    /// <remarks>
    /// This exception is used to handle errors specific to SMTP client, such as connection failures,
    /// message sending issues, or authentication problems. It provides utility methods for conditional exception throwing.
    /// </remarks>
    public class SmtpClientException : SenderException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpClientException"/> class.
        /// </summary>
        public SmtpClientException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpClientException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public SmtpClientException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpClientException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public SmtpClientException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
