using Amazon.S3;

namespace AirSoft.S3
{
    /// <summary>
    /// Class for configuring Amazon S3 settings.
    /// </summary>
    public class S3ConfigureOptions
    {
        /// <summary>
        /// Gets or sets the access key for Amazon S3.
        /// </summary>
        /// <value>The access key for Amazon S3.</value>
        public string AccessKey { internal get; set; } = null!;

        /// <summary>
        /// Gets or sets the secret key for Amazon S3.
        /// </summary>
        /// <value>The secret key for Amazon S3.</value>
        public string SecretKey { internal get; set; } = null!;

        /// <summary>
        /// Gets or sets the region for Amazon S3.
        /// </summary>
        /// <value>The region for Amazon S3.</value>
        public string Region { internal get; set; } = null!;

        /// <summary>
        /// Advanced AmazonS3Config settings (e.g., ForcePathStyle, ServiceURL, Proxy settings).
        /// </summary>
        /// <value>AmazonS3Config settings.</value>
        public AmazonS3Config S3Config { internal get; set; } = null!;
    }
}
