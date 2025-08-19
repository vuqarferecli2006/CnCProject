using CnC.Application.Abstracts.Services;
using CnC.Application.Shared.Helpers.RoleHelpers;
using CnC.Infrastructure.Services;
using CnC.Infrastructure.Services.EmailRabbitMQ;
using Microsoft.Extensions.DependencyInjection;

namespace CnC.Percistance;

public static class ServiceRegistration
{
    public static void RegisterServices(this IServiceCollection services)
    {


        #region Repository

        #endregion




        #region Services
            services.AddSingleton<EmailConsumer>();
            services.AddScoped<RoleCreationHelper>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IEmailQueueService, RabbitMqEmailQueueService>();
            //services.AddHostedService<EmailConsumerService>();

        #endregion
    }
}
