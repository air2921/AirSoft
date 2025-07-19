using AirSoft.Cryptography.Abstractions;
using AirSoft.Cryptography.Abstractions.Enums;
using AirSoft.Exceptions;
using BCrypt.Net;
using System;
using static BCrypt.Net.BCrypt;

namespace AirSoft.Cryptography.Hasher
{
    /// <summary>
    /// A class responsible for hashing and verifying strings using the BCrypt algorithm.
    /// This class implements the <see cref="IHasher"/> interface to provide hashing and verification functionality.
    /// </summary>
    /// <remarks>
    /// This class uses the BCrypt algorithm to hash and verify strings. It supports enhanced hashing with different hash types,
    /// such as SHA512. Errors during hashing or verification are logged using the provided logger.
    /// </remarks>
    public class BCryptHasher : IHasher
    {
        /// <summary>
        /// Hashes a string using the specified hash algorithm.
        /// </summary>
        /// <param name="src">The string to hash.</param>
        /// <param name="hashAlgorithm">The hash algorithm to use for enhanced hashing. Defaults to <see cref="HashAlgorithm.SHA512"/>.</param>
        /// <returns>A hashed string representation of the source string.</returns>
        /// <exception cref="CryptographyException">Thrown when an error occurs during the hashing process (e.g., invalid algorithm or other cryptographic issues).</exception>
        public string Hash(string src, HashAlgorithm hashAlgorithm = HashAlgorithm.SHA512)
        {
            try
            {
                return EnhancedHashPassword(src, ConvertToHashType(hashAlgorithm));
            }
            catch (Exception ex)
            {
                throw new CryptographyException("An error occurred while hashing the object", ex);
            }
        }

        /// <summary>
        /// Verifies if an input string matches a previously hashed string.
        /// </summary>
        /// <param name="input">The input string to verify (e.g., a password).</param>
        /// <param name="src">The previously hashed string to compare against.</param>
        /// <param name="hashAlgorithm">The hash algorithm used for enhanced verification. Defaults to <see cref="HashAlgorithm.SHA512"/>.</param>
        /// <returns><c>true</c> if the input matches the hashed string; otherwise, <c>false</c>.</returns>
        /// <exception cref="CryptographyException">Thrown when an error occurs during the verification process (e.g., invalid algorithm or cryptographic failure).</exception>
        public bool Verify(string input, string src, HashAlgorithm hashAlgorithm = HashAlgorithm.SHA512)
        {
            try
            {
                return EnhancedVerify(input, src, ConvertToHashType(hashAlgorithm));
            }
            catch (Exception ex)
            {
                throw new CryptographyException("An error occurred while checking the hash", ex);
            }
        }

        /// <summary>
        /// Converts <see cref="HashAlgorithm"/> to the corresponding <see cref="HashType"/>
        /// </summary>
        /// <param name="hashAlgorithm">Source hash algorithm enum</param>
        /// <returns>Matching <see cref="HashType"/> or <see cref="HashType.None"/> if no match</returns>
        private static HashType ConvertToHashType(HashAlgorithm hashAlgorithm)
        {
            if (hashAlgorithm == HashAlgorithm.SHA256)
                return HashType.SHA256;
            if (hashAlgorithm == HashAlgorithm.SHA384)
                return HashType.SHA384;
            if (hashAlgorithm == HashAlgorithm.SHA512)
                return HashType.SHA512;

            return HashType.None;
        }
    }
}
