using CnC.Application.Abstracts.Repositories.ICategoryRepositories;
using CnC.Application.Abstracts.Repositories.ICurrencyRateRepository;
using CnC.Application.Abstracts.Repositories.IProductCurrencyRepository;
using CnC.Application.Abstracts.Repositories.IProductDescriptionRepository;
using CnC.Application.Abstracts.Repositories.IProductFilesRepository;
using CnC.Application.Abstracts.Repositories.IProductRepositories;
using CnC.Application.Abstracts.Services;
using CnC.Application.Shared.Helpers.RoleHelpers;
using CnC.Infrastructure.Services;
using CnC.Infrastructure.Services.EmailRabbitMQ;
using CnC.Percistance.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CnC.Percistance;

public static class ServiceRegistration
{
    public static void RegisterServices(this IServiceCollection services)
    {


        #region Repository
            services.AddScoped<ICategoryReadRepository, CategoryRepository>();
            services.AddScoped<ICategoryWriteRepository, CategoryRepository>();
            services.AddScoped<IProductReadRepository, ProductRepository>();
            services.AddScoped<IProductWriteRepository, ProductRepository>();
            services.AddScoped<IProductCurrencyWriteRepository, ProductCurrencyRepository>();
            services.AddScoped<IProductCurrencyReadRepository, ProductCurrencyRepository>();
            services.AddScoped<IProductDescriptionReadRepository, ProductDescriptionRepository>();
            services.AddScoped<IProductDescriptionWriteRepository, ProductDescriptionRepository>();
            services.AddScoped<ICurrencyRateReadRepository, CurrencyRateRepository>();
            services.AddScoped<ICurrencyRateWriteRepository, CurrencyRateRepository>();
            services.AddScoped<IProductFilesWriteRepository, ProductFilesRepository>();
        #endregion




        #region Services
        services.AddSingleton<EmailConsumer>();
            services.AddScoped<RoleCreationHelper>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IEmailQueueService, RabbitMqEmailQueueService>();
            services.AddScoped<ITokenBlacklistService, InMemoryTokenBlacklistService>();
            services.AddScoped<IFileServices, CloudinaryUploadService>();
            services.AddTransient<CurrencyUpdateJob>();
            services.AddHttpContextAccessor();

        //services.AddHostedService<EmailConsumerService>();  //RabbitMQ email consumer service

        #endregion
    }
}
