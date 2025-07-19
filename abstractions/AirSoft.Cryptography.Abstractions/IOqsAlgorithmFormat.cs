namespace AirSoft.Cryptography.Abstractions
{
    /// <summary>
    /// Specifies the format and parameters for a quantum-safe cryptographic algorithm
    /// </summary>
    /// <remarks>
    /// Default implementation includes embedded OQS library path.
    /// This interface provides platform-specific details for working with Open Quantum Safe (OQS) cryptographic algorithms.
    /// </remarks>
    public interface IOqsAlgorithmFormat
    {
        /// <summary>
        /// Gets the name of the quantum-safe algorithm (e.g., "Dilithium5")
        /// </summary>
        string Algorithm { get; }

        /// <summary>
        /// Gets the length of signatures (in bytes) produced by this algorithm
        /// </summary>
        int SignatureLength { get; }

        /// <summary>
        /// Gets the length of public keys (in bytes) for this algorithm
        /// </summary>
        int PublicKeyLength { get; }

        /// <summary>
        /// Gets the length of private keys (in bytes) for this algorithm
        /// </summary>
        int PrivateKeyLength { get; }
    }
}