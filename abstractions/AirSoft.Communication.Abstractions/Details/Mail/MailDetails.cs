using AirSoft.Communication.Abstractions.Enums;

namespace AirSoft.Communication.Abstractions.Details.Mail
{
    /// <summary>
    /// Represents a complete email message with all components
    /// </summary>
    /// <remarks>
    /// Contains all necessary information to construct and send an email,
    /// including recipients, subject, body content, and metadata.
    /// </remarks>
    public class MailDetails : IMessage
    {
        /// <summary>
        /// Email address of the primary recipient
        /// </summary>
        public required string To { get; set; }

        /// <summary>
        /// Display name of the primary recipient
        /// </summary>
        public required string UsernameTo { get; set; }

        /// <summary>
        /// Subject line of the email
        /// </summary>
        /// <remarks>
        /// Will be properly encoded for special characters
        /// </remarks>
        public required string Subject { get; set; }

        /// <summary>
        /// Email body content and attachments
        /// </summary>
        /// <remarks>
        /// Optional - can be null for notification-only emails
        /// </remarks>
        public MailBodyDetails? Body { get; set; }

        /// <summary>
        /// Collection of custom email headers
        /// </summary>
        /// <remarks>
        /// Useful for adding metadata or controlling email processing
        /// </remarks>
        public ICollection<MailHeaderDetails> Headers { get; set; } = [];

        /// <summary>
        /// Priority level of the email
        /// </summary>
        /// <remarks>
        /// Affects email client display (not delivery speed)
        /// </remarks>
        public MailPriority Priority { get; set; } = MailPriority.Normal;
    }
}
