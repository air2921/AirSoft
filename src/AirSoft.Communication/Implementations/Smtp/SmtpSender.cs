using AirSoft.Communication.Abstractions;
using AirSoft.Communication.Abstractions.Details.Mail;
using AirSoft.Communication.Extensions;
using AirSoft.Communication.Options;
using AirSoft.Exceptions;

namespace AirSoft.Communication.Implementations.Smtp
{
    /// <summary>
    /// A class responsible for sending emails using an SMTP client. This class implements the <see cref="ISender{MailDetails}"/> interface to send emails asynchronously.
    /// </summary>
    /// <param name="configureOptions">Configuration options containing SMTP provider settings like the sender's name and address.</param>
    /// <param name="smtpClient">An instance of <see cref="SmtpClientWrapper"/> that handles the actual email sending process.</param>
    /// <remarks>
    /// This class constructs an email message using the provided details and forwards it to the <see cref="SmtpClientWrapper"/> to send the email.
    /// It handles errors during the email construction and sending process and logs any exceptions for further analysis.
    /// </remarks>
    public class SmtpSender(SmtpConfigureOptions configureOptions, SmtpClientWrapper smtpClient) : ISender<MailDetails>
    {
        /// <summary>
        /// Asynchronously sends an email using the provided <see cref="MailDetails"/> object.
        /// </summary>
        /// <param name="mail">An object containing the details of the email to send, including recipient, subject, and body.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <exception cref="SmtpClientException">Thrown if an error occurs while sending the email or client was disposed.</exception>
        public async Task SendAsync(MailDetails mail, CancellationToken cancellationToken = default)
        {
            try
            {
                using var message = mail.ToMailKitMessage(configureOptions.SenderName, configureOptions.Address);
                await smtpClient.SendAsync(message, cancellationToken);
            }
            catch (Exception ex) when (ex is not SmtpClientException)
            {
                throw new SmtpClientException("An error occurred while sending the email", ex);
            }
        }

        /// <summary>
        /// Sends an email synchronously using the provided <see cref="MailDetails"/> object.
        /// </summary>
        /// <param name="mail">An object containing the details of the email to send, including recipient, subject, and body.</param>
        /// <exception cref="SmtpClientException">Thrown if an error occurs while sending the email or client was disposed.</exception>
        public void Send(MailDetails mail)
        {
            try
            {
                using var message = mail.ToMailKitMessage(configureOptions.SenderName, configureOptions.Address);
                smtpClient.Send(message);
            }
            catch (Exception ex) when (ex is not SmtpClientException)
            {
                throw new SmtpClientException("An error occurred while sending the email", ex);
            }
        }
    }
}
