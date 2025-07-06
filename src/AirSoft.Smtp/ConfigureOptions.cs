using AirSoft.Details.Enums;
using MailKit.Security;

namespace AirSoft.Smtp
{
    /// <summary>
    /// Class for configuring SMTP (Simple Mail Transfer Protocol) settings.
    /// </summary>
    public class ConfigureOptions
    {
        /// <summary>
        /// Gets or sets the SMTP provider (e.g., Gmail, Outlook, etc.).
        /// </summary>
        /// <value>The SMTP provider.</value>
        public string Provider { internal get; set; } = null!;

        /// <summary>
        /// Gets or sets the sender's display name for emails.
        /// </summary>
        /// <value>The sender's display name.</value>
        public string SenderName { internal get; set; } = null!;

        /// <summary>
        /// Gets or sets the email address used for sending emails.
        /// </summary>
        /// <value>The email address.</value>
        public string Address { internal get; set; } = null!;

        /// <summary>
        /// Gets or sets the password for the email account.
        /// </summary>
        /// <value>The email account password.</value>
        public string Password { internal get; set; } = null!;

        /// <summary>
        /// Gets or sets the port number for the SMTP server.
        /// </summary>
        /// <value>The SMTP server port.</value>
        public SmtpPort Port { internal get; set; }

        /// <summary>
        /// Gets or sets the secure socket options for SMTP connection.
        /// </summary>
        /// <value>The SSL/TLS connection options. Default is Auto.</value>
        public SecureSocketOptions SecureSocket { internal get; set; } = SecureSocketOptions.Auto;
    }
}
