using AirSoft.Communication.Abstractions.Enums;

namespace AirSoft.Communication.Abstractions.Details.Mail
{
    /// <summary>
    /// Represents the body content of an email message
    /// </summary>
    /// <remarks>
    /// Supports both plain text and HTML content, along with attachments
    /// and embedded resources. At least one body type (Text or HTML)
    /// should be provided.
    /// </remarks>
    public class MailBodyDetails
    {
        /// <summary>
        /// Plain text content of the email
        /// </summary>
        /// <remarks>
        /// Will be used as fallback if HTML content is unavailable
        /// </remarks>
        public string? TextBody { get; set; }

        /// <summary>
        /// HTML content of the email
        /// </summary>
        /// <remarks>
        /// Can reference embedded resources using cid: references
        /// </remarks>
        public string? HtmlBody { get; set; }

        /// <summary>
        /// Collection of file attachments
        /// </summary>
        public ICollection<MailAttachmentDetails> Attachments { get; set; } = [];

        /// <summary>
        /// Collection of embedded resources (e.g., images for HTML)
        /// </summary>
        public ICollection<MailResourceDetails> Resources { get; set; } = [];

        /// <summary>
        /// Content transfer encoding for the body
        /// </summary>
        /// <remarks>
        /// Defaults to 7-bit for plain text, but will automatically
        /// switch to quoted-printable if extended characters are present
        /// </remarks>
        public ContentTransferEncoding ContentTransferEncoding { get; set; } = ContentTransferEncoding.SevenBit;
    }
}
