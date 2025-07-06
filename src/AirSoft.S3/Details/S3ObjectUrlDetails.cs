using Amazon.S3;

namespace AirSoft.S3.Details
{
    /// <summary>
    /// Represents details of a pre-signed URL for temporary access to an S3 object.
    /// </summary>
    /// <remarks>
    /// Used for generating time-limited access URLs that can be shared without requiring AWS credentials.
    /// The <see cref="Url"/> becomes invalid after the <see cref="Expires"/> timestamp.
    /// </remarks>
    public class S3ObjectUrlDetails
    {
        /// <summary>
        /// Gets or sets the object key representing the full path within the S3 bucket.
        /// </summary>
        public string Key { get; set; } = null!;

        /// <summary>
        /// Gets or sets the generated pre-signed URL for accessing the object.
        /// </summary>
        public string Url { get; set; } = null!;

        /// <summary>
        /// Gets or sets the expiration date and time of the pre-signed URL.
        /// </summary>
        /// <remarks>
        /// All times are in UTC. Attempts to use the URL after this time will return 403 Forbidden.
        /// </remarks>
        public DateTime Expires { get; set; }

        /// <summary>
        /// Gets or sets the HTTP verb permitted by the pre-signed URL.
        /// </summary>
        /// <remarks>
        /// Typically GET for downloads or PUT for uploads. Other verbs may be restricted by S3 policies.
        /// </remarks>
        public HttpVerb Verb { get; set; }
    }
}
