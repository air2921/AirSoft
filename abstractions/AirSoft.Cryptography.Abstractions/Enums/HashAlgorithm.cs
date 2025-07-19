namespace AirSoft.Cryptography.Abstractions.Enums
{
    /// <summary>
    /// Specifies the cryptographic hash algorithm to use for security operations
    /// </summary>
    /// <remarks>
    /// These algorithms are part of the SHA-2 family and provide different levels of security:
    /// <list type="bullet">
    /// <item><term>SHA256</term><description>Produces a 256-bit (32-byte) hash value (fastest, standard security)</description></item>
    /// <item><term>SHA384</term><description>Produces a 384-bit (48-byte) hash value (balanced security)</description></item>
    /// <item><term>SHA512</term><description>Produces a 512-bit (64-byte) hash value (highest security)</description></item>
    /// </list>
    /// </remarks>
    public enum HashAlgorithm
    {
        /// <summary>
        /// SHA-256 algorithm (FIPS 180-4)
        /// </summary>
        /// <remarks>
        /// Recommended for most general-purpose hashing needs. Provides 128-bit collision resistance.
        /// </remarks>
        SHA256,

        /// <summary>
        /// SHA-384 algorithm (FIPS 180-4)
        /// </summary>
        /// <remarks>
        /// Provides 192-bit collision resistance. Often used in TLS/SSL implementations.
        /// </remarks>
        SHA384,

        /// <summary>
        /// SHA-512 algorithm (FIPS 180-4)
        /// </summary>
        /// <remarks>
        /// Provides 256-bit collision resistance. Used when maximum security is required.
        /// </remarks>
        SHA512
    }
}
