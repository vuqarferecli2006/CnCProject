using CnC.Application.Abstracts.Repositories.IBasketRepositories;
using CnC.Application.Abstracts.Repositories.ICategoryRepositories;
using CnC.Application.Abstracts.Repositories.ICurrencyRateRepository;
using CnC.Application.Abstracts.Repositories.IDownloadRepositories;
using CnC.Application.Abstracts.Repositories.IFavouriteRepositories;
using CnC.Application.Abstracts.Repositories.IInformationModelRepositories;
using CnC.Application.Abstracts.Repositories.IOrderProductRepositories;
using CnC.Application.Abstracts.Repositories.IOrderRepositories;
using CnC.Application.Abstracts.Repositories.IPaymentMethodRepositories;
using CnC.Application.Abstracts.Repositories.IPaymentRepositories;
using CnC.Application.Abstracts.Repositories.IProductBasketRepositories;
using CnC.Application.Abstracts.Repositories.IProductCurrencyRepository;
using CnC.Application.Abstracts.Repositories.IProductDescriptionRepository;
using CnC.Application.Abstracts.Repositories.IProductFilesRepository;
using CnC.Application.Abstracts.Repositories.IProductRepositories;
using CnC.Application.Abstracts.Repositories.IProductViewRepositories;
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
            services.AddScoped<IFavouriteReadRepository, FavouriteRepository>();
            services.AddScoped<IFavouriteWriteRepository, FavouriteRepository>();
            services.AddScoped<IProductViewReadRepository, ProductViewRepository>();
            services.AddScoped<IProductViewWriteRepository, ProductViewRepository>();
            services.AddScoped<IBasketReadRepository, BasketRepository>();
            services.AddScoped<IBasketWriteRepository, BasketRepository>();
            services.AddScoped<IProductBasketReadRepository, ProductBasketRepository>();
            services.AddScoped<IProductBasketWriteRepository, ProductBasketRepository>();
            services.AddScoped<IOrderReadRepository, OrderRepository>();
            services.AddScoped<IOrderWriteRepository, OrderRepository>();
            services.AddScoped<IOrderProductReadRepository, OrderProductRepository>();
            services.AddScoped<IOrderProductWriteRepository, OrderProductRepository>();
            services.AddScoped<IPaymentReadRepository, PaymentRepository>();
            services.AddScoped<IPaymentWriteRepository, PaymentRepository>();
            services.AddScoped<IPaymentMethodReadRepository, PaymentMethodRepository>();
            services.AddScoped<IPaymentMethodWriteRepository, PaymentMethodRepository>();
            services.AddScoped<IDownloadReadRepository, DownloadRepository>();
            services.AddScoped<IDownloadWriteRepository, DownloadRepository>();
            services.AddScoped<IInformationModelReadRepository,InformationModelRepository>();
            services.AddScoped<IInformationModelWriteRepository,InformationModelRepository>();

        #endregion




        #region Services

            services.AddSingleton<EmailConsumer>();
            services.AddScoped<RoleCreationHelper>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IEmailQueueService, RabbitMqEmailQueueService>();
            services.AddScoped<ITokenBlacklistService, InMemoryTokenBlacklistService>();
            services.AddScoped<IFileServices, CloudinaryUploadService>();
            services.AddScoped<IElasticProductService, ElasticProductService>();
            services.AddTransient<CurrencyUpdateJob>();
            services.AddHttpContextAccessor();
            services.AddScoped<IStripePaymentService, StripeService>();
            services.AddHostedService<EmailConsumerService>();  //RabbitMQ email consumer service

        #endregion
    }
}
