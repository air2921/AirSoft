using System;
using System.Runtime.InteropServices;

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
        /// The namespace where OQS library resources are embedded
        /// </summary>
        /// <value>Returns "AirSoft.Cryptography.Assembly" as the default resource namespace</value>
        public const string ResourceNamespace = "AirSoft.Cryptography.Assembly";

        /// <summary>
        /// Gets the platform-specific filename for the OQS library
        /// </summary>
        /// <value>
        /// Returns "oqs.dll" on Windows, "oqs.so" on Linux, or "oqs.dylib" on macOS
        /// </value>
        public string FileName => DefineOqsFile();

        /// <summary>
        /// Gets the full resource path for the embedded OQS library
        /// </summary>
        /// <value>
        /// Returns the resource path combining <see cref="ResourceNamespace"/> with the platform-specific filename
        /// </value>
        public string ResourceName => DefineOqsResource();

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

        /// <summary>
        /// Determines the appropriate OQS (Open Quantum Safe) library filename based on the current operating system.
        /// </summary>
        /// <returns>The platform-specific OQS library filename.</returns>
        /// <exception cref="PlatformNotSupportedException">
        /// Thrown when the current platform is unsupported.
        /// </exception>
        private static string DefineOqsFile()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return "oqs.dll";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return "oqs.so";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return "oqs.dylib";

            throw new PlatformNotSupportedException("Oqs is not supported on this platform");
        }

        /// <summary>
        /// Determines the appropriate OQS (Open Quantum Safe) library resource path based on the current operating system.
        /// </summary>
        /// <returns>The platform-specific OQS library resource path.</returns>
        /// <exception cref="PlatformNotSupportedException">
        /// Thrown when the current platform is unsupported.
        /// </exception>
        private static string DefineOqsResource()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return $"{ResourceNamespace}.oqs.dll";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return $"{ResourceNamespace}.oqs.so";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return $"{ResourceNamespace}.oqs.dylib";

            throw new PlatformNotSupportedException("Oqs is not supported on this platform");
        }
    }

}