using AirSoft.Communication.Abstractions.Enums;

namespace AirSoft.Communication.Abstractions.Details.Mail
{
    /// <summary>
    /// Represents an email attachment or embedded resource
    /// </summary>
    /// <remarks>
    /// Can be used either as a traditional attachment (downloadable file)
    /// or as an inline resource (displayed within email body).
    /// The <see cref="Content"/> stream ownership is transferred to MIME processor.
    /// </remarks>
    public class MailAttachmentDetails
    {
        /// <summary>
        /// Unique content identifier for inline resources
        /// </summary>
        /// <remarks>
        /// Required when <see cref="IsInline"/> is true.
        /// Referenced in HTML as cid:ContentId (e.g., &lt;img src="cid:logo"&gt;)
        /// </remarks>
        public required string ContentId { get; set; }

        /// <summary>
        /// Name of the attached file
        /// </summary>
        /// <example>"report.pdf"</example>
        public required string FileName { get; set; }

        /// <summary>
        /// Stream containing attachment data
        /// </summary>
        /// <remarks>
        /// <para>Will be disposed by the MIME processor after sending.</para>
        /// <para>For files, use FileStream; for in-memory data, use MemoryStream.</para>
        /// </remarks>
        public required Stream Content { get; set; }

        /// <summary>
        /// MIME type of the attachment
        /// </summary>
        /// <example>"application/pdf"</example>
        public required string ContentType { get; set; }

        /// <summary>
        /// Determines if the file should be displayed inline
        /// </summary>
        /// <remarks>
        /// True for embedded resources (e.g., images in HTML),
        /// False for regular attachments (default)
        /// </remarks>
        public bool IsInline { get; set; } = false;

        /// <summary>
        /// Content transfer encoding method
        /// </summary>
        /// <remarks>
        /// Defaults to Base64 for binary attachments.
        /// Use QuotedPrintable for text attachments with special characters.
        /// </remarks>
        public ContentTransferEncoding ContentTransferEncoding { get; set; } = ContentTransferEncoding.Base64;
    }
}
