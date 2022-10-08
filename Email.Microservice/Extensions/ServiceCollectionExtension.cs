using Email.Microservice.Application.Services;
using Email.Microservice.BackgroundServices;
using Email.Microservice.Core.Interfaces;

namespace Email.Microservice.Extensions
{
    public static class ServiceCollectionExtension
    {  /// <summary>
       /// Inject Services 
       /// </summary>
       /// <param name="services"></param>
       /// <returns></returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IEmailService, EmailService>();
            services.AddHostedService<RabbitMQBackgroundConsumerService>();

            return services;
        }
    }
}
