using AirSoft.Communication.Abstractions.Enums;

namespace AirSoft.Communication.Abstractions.Details.Mail
{
    /// <summary>
    /// Represents an embedded email resource (e.g., images in HTML)
    /// </summary>
    /// <remarks>
    /// Used for content referenced via CID in HTML (like &lt;img src="cid:ContentId"&gt;).
    /// Unlike attachments, these are displayed inline.
    /// Note: The Stream is owned by MimeContent after construction.
    /// </remarks>
    public class MailResourceDetails
    {
        /// <summary>
        /// Unique identifier for HTML reference (e.g., "logo" for cid:logo)
        /// </summary>
        public required string ContentId { get; set; }

        /// <summary>
        /// Resource data stream (PNG, JPEG, etc.)
        /// Warning: Ownership transfers to MIME processor
        /// </summary>
        public required Stream Content { get; set; }

        /// <summary>
        /// MIME type (e.g., "image/png")
        /// </summary>
        public required string ContentType { get; set; }
    }
}
