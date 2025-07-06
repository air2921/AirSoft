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
        /// Gets or sets the endpoint for the S3 storage provider.
        /// </summary>
        /// <value>The endpoint for the S3 storage provider.</value>
        /// <remarks>
        /// This property is required for Yandex Object Storage and should be set to the provider's endpoint (e.g., "https://storage.yandexcloud.net").
        /// For Amazon S3, this property is required.
        /// </remarks>
        public string Endpoint { internal get; set; } = null!;
    }
}
