using AirSoft.Communication.Abstractions;
using AirSoft.Communication.Details;
using AirSoft.Communication.Implementations.Sms;
using AirSoft.Communication.Implementations.Smtp;
using AirSoft.Communication.Options;
using Microsoft.Extensions.DependencyInjection;
using Twilio;

namespace AirSoft.Communication
{
    public static class ServiceExtension
    {
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
