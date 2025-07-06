using AirSoft.Exceptions;
using AirSoft.S3.Details;
using Amazon.S3;

namespace AirSoft.S3.Abstractions
{
    /// <summary>
    /// Defines methods for interacting with an S3-compatible storage service with detailed metadata support.
    /// Provides functionality for generating pre-signed URLs, downloading with metadata, uploading, and removing objects.
    /// </summary>
    public interface IS3Client
    {
        /// <summary>
        /// Generates a pre-signed URL with comprehensive access details for an S3 object.
        /// </summary>
        /// <param name="bucket">The name of the S3 bucket containing the object.</param>
        /// <param name="key">The key (path) of the object in the bucket.</param>
        /// <param name="verb">The http verb for creating url.</param>
        /// <param name="expires">The expiration date and time of the pre-signed URL.</param>
        /// <returns>
        /// A <see cref="ValueTask{S3ObjectUrlDetails}"/> containing:
        /// - Generated URL (Url)
        /// - Object key (Key)
        /// - HTTP method (Verb)
        /// - Expiration timestamp (Expires)
        /// </returns>
        /// <exception cref="S3ClientException">Thrown when URL generation fails.</exception>
        public Task<S3ObjectUrlDetails> SignUrlAsync(string bucket, string key, HttpVerb verb, DateTime expires);

        /// <summary>
        /// Downloads an object with complete content metadata from S3 storage.
        /// </summary>
        /// <param name="bucket">The name of the S3 bucket containing the object.</param>
        /// <param name="key">The key (path) of the object in the bucket.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation (optional).</param>
        /// <returns>
        /// A <see cref="Task{S3ObjectContentDetails}"/> containing:
        /// - Object content stream (Content)
        /// - Object key (Key)
        /// - Content size in bytes (Size)
        /// - MIME type (ContentType)
        /// </returns>
        /// <exception cref="S3ClientException">Thrown when download fails.</exception>
        public Task<S3ObjectContentDetails> DownloadAsync(string bucket, string key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Uploads a stream to S3 storage.
        /// </summary>
        /// <param name="stream">The data stream to upload.</param>
        /// <param name="bucket">The target S3 bucket name.</param>
        /// <param name="key">The destination key (path) in the bucket.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation (optional).</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="S3ClientException">Thrown when upload fails.</exception>
        public Task UploadAsync(Stream stream, string bucket, string key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes an object from S3 storage.
        /// </summary>
        /// <param name="bucket">The S3 bucket containing the object.</param>
        /// <param name="key">The key (path) of the object to remove.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation (optional).</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="S3ClientException">Thrown when removal fails.</exception>
        public Task RemoveAsync(string bucket, string key, CancellationToken cancellationToken = default);
    }
}