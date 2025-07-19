using AirSoft.Communication.Abstractions;
using AirSoft.Communication.Abstractions.Details.Mail;
using AirSoft.Communication.Abstractions.Details.Telecom;
using AirSoft.Communication.Implementations.Sms;
using AirSoft.Communication.Implementations.Smtp;
using AirSoft.Communication.Options;
using Microsoft.Extensions.DependencyInjection;
using Twilio;

namespace AirSoft.Communication
{
    /// <summary>
    /// Provides extension methods for configuring services in <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// Adds a Telecom client (e.g., Twilio) to the dependency injection container.
        /// </summary>
        /// <param name="services">The service collection to add the client to.</param>
        /// <param name="action">Configuration action for <see cref="TelecomConfigureOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> for chaining.</returns>
        /// <remarks>
        /// Initializes the Twilio client with provided credentials and registers:
        /// - <see cref="TelecomConfigureOptions"/> as singleton
        /// - <see cref="SmsClientWrapper"/> as singleton
        /// - <see cref="ISender{SmsDetails}"/> implementation as scoped
        /// </remarks>
        public static IServiceCollection AddTelecomClient(this IServiceCollection services, Action<TelecomConfigureOptions> action)
        {
            var options = new TelecomConfigureOptions();
            action.Invoke(options);

            TwilioClient.Init(options.Username, options.Password, options.AccountSid);

            services.AddSingleton(options);
            services.AddSingleton<SmsClientWrapper>();
            services.AddScoped<ISender<SmsDetails>, SmsSender>();

            return services;
        }

        /// <summary>
        /// Adds an SMTP client to the dependency injection container.
        /// </summary>
        /// <param name="services">The service collection to add the client to.</param>
        /// <param name="action">Configuration action for <see cref="SmtpConfigureOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> for chaining.</returns>
        /// <remarks>
        /// Registers:
        /// - <see cref="SmtpConfigureOptions"/> as singleton
        /// - <see cref="SmtpClientWrapper"/> as scoped
        /// - <see cref="ISender{MailDetails}"/> implementation as scoped
        /// </remarks>
        public static IServiceCollection AddSmtpClient(this IServiceCollection services, Action<SmtpConfigureOptions> action)
        {
            var options = new SmtpConfigureOptions();
            action.Invoke(options);

            services.AddSingleton(options);
            services.AddScoped<SmtpClientWrapper>();
            services.AddScoped<ISender<MailDetails>, SmtpSender>();

            return services;
        }
    }
}
