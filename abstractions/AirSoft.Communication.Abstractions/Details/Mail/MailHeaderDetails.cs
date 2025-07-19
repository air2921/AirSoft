namespace AirSoft.Communication.Abstractions.Details.Mail
{
    /// <summary>
    /// Represents a custom email header for messages
    /// </summary>
    /// <remarks>
    /// Allows adding non-standard or custom headers to email messages.
    /// Headers will be included in the final MIME message.
    /// </remarks>
    public class MailHeaderDetails
    {
        /// <summary>
        /// Header name (e.g., "X-Custom-Header")
        /// </summary>
        /// <remarks>
        /// Standard headers should use proper casing (e.g., "Message-ID" not "message-id")
        /// </remarks>
        public required string Name { get; set; }

        /// <summary>
        /// Header value
        /// </summary>
        /// <remarks>
        /// Will be properly encoded according to MIME standards
        /// </remarks>
        public required string Value { get; set; }
    }
}
