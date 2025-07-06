using AirSoft.Exceptions.Base;
using System;

namespace AirSoft.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an error occurs during cryptographic operations.
    /// </summary>
    /// <remarks>
    /// This exception is used to handle errors specific to cryptography, such as encryption, decryption,
    /// or key management failures. It provides utility methods for conditional exception throwing.
    /// </remarks>
    public class CryptographyException : AirSoftException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CryptographyException"/> class.
        /// </summary>
        public CryptographyException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptographyException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public CryptographyException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptographyException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public CryptographyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
