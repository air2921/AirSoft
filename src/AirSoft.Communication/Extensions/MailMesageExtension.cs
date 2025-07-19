using AirSoft.Communication.Abstractions.Details.Mail;
using AirSoft.Communication.Abstractions.Enums;
using MimeKit;

namespace AirSoft.Communication.Extensions
{
    /// <summary>
    /// Provides extension methods for converting <see cref="MailDetails"/> to MimeKit's <see cref="MimeMessage"/>
    /// </summary>
    internal static class MailMessageExtension
    {
        /// <summary>
        /// Converts <see cref="MailDetails"/> to a MimeKit <see cref="MimeMessage"/>
        /// </summary>
        /// <param name="mail">The mail details to convert</param>
        /// <param name="senderName">Display name of the sender</param>
        /// <param name="senderFrom">Email address of the sender</param>
        /// <returns>A fully constructed <see cref="MimeMessage"/> ready for sending</returns>
        /// <remarks>
        /// <para>
        /// This extension handles:
        /// <list type="bullet">
        /// <item><description>Recipient addressing with display names</description></item>
        /// <item><description>Priority header conversion</description></item>
        /// <item><description>Custom header inclusion</description></item>
        /// <item><description>Multipart body construction (text/HTML/attachments)</description></item>
        /// </list>
        /// </para>
        /// <para>
        /// Note: Transfers ownership of all <see cref="Stream"/> resources to the MimeMessage.
        /// </para>
        /// </remarks>
        internal static MimeMessage ToMailKitMessage(this MailDetails mail, string senderName, string senderFrom)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(senderName, senderFrom));
            message.To.Add(new MailboxAddress(mail.UsernameTo, mail.To));
            message.Subject = mail.Subject;

            message.Headers.Add("X-Priority", ((int)mail.Priority).ToString());

            foreach (var header in mail.Headers)
                message.Headers.Add(header.Name, header.Value);

            var builder = new BodyBuilder();

            if (mail.Body != null)
            {
                if (!string.IsNullOrEmpty(mail.Body.TextBody))
                    builder.TextBody = mail.Body.TextBody;

                if (!string.IsNullOrEmpty(mail.Body.HtmlBody))
                    builder.HtmlBody = mail.Body.HtmlBody;

                foreach (var attachment in mail.Body.Attachments)
                {
                    builder.Attachments.Add(
                        attachment.FileName,
                        attachment.Content,
                        ContentType.Parse(attachment.ContentType));
                }

                foreach (var resource in mail.Body.Resources)
                {
                    builder.LinkedResources.Add(
                        resource.ContentId,
                        resource.Content,
                        ContentType.Parse(resource.ContentType));
                }
            }

            message.Body = builder.ToMessageBody();
            return message;
        }
    }
}
