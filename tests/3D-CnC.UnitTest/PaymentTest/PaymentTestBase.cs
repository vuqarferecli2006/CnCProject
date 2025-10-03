using CnC.Application.Abstracts.Repositories.ICurrencyRateRepository;
using CnC.Application.Abstracts.Repositories.IDownloadRepositories;
using CnC.Application.Abstracts.Repositories.IOrderRepositories;
using CnC.Application.Abstracts.Repositories.IPaymentMethodRepositories;
using CnC.Application.Abstracts.Repositories.IPaymentRepositories;
using CnC.Application.Abstracts.Repositories.IProductRepositories;
using CnC.Application.Abstracts.Services;
using CnC.Application.Features.Payment.Commands;
using CnC.Domain.Entities;
using CnC.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace _3D_CnC.UnitTest.PaymentTest;

public abstract class PaymentTestBase
{
    protected readonly Mock<IHttpContextAccessor> HttpContextAccessorMock = new();
    protected readonly Mock<IOrderReadRepository> OrderReadRepoMock = new();
    protected readonly Mock<IOrderWriteRepository> OrderWriteRepoMock = new();
    protected readonly Mock<IPaymentMethodReadRepository> PaymentMethodReadRepoMock = new();
    protected readonly Mock<IPaymentWriteRepository> PaymentWriteRepoMock = new();
    protected readonly Mock<ICurrencyRateReadRepository> CurrencyRateReadRepoMock = new();
    protected readonly Mock<IPaymentStrategyFactory> PaymentStrategyFactoryMock = new();
    protected readonly Mock<IProductWriteRepository> ProductWriteRepoMock = new();
    protected readonly Mock<IElasticProductService> ElasticProductServiceMock = new();
    protected readonly Mock<IDownloadWriteRepository> DownloadWriteRepoMock = new();
    protected readonly Mock<IEmailQueueService> EmailQueueServiceMock = new();

    protected CreatePaymentCommandHandler Handler => new(
        HttpContextAccessorMock.Object,
        OrderReadRepoMock.Object,
        CurrencyRateReadRepoMock.Object,
        PaymentMethodReadRepoMock.Object,
        PaymentWriteRepoMock.Object,
        PaymentStrategyFactoryMock.Object,
        OrderWriteRepoMock.Object,
        ProductWriteRepoMock.Object,
        ElasticProductServiceMock.Object,
        DownloadWriteRepoMock.Object,
        EmailQueueServiceMock.Object
    );

    #region Helpers

    protected static Order CreateTestOrder(string userId, decimal totalAmount = 100m)
        => new()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            isPaid = false,
            TotalAmount = totalAmount,
            OrderProducts = new List<OrderProduct> { new() { Id = Guid.NewGuid(), ProductId = Guid.NewGuid() } }
        };

    protected static PaymentMethod CreatePaymentMethod(string userId)
        => new()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            MethodForPayment = MethodForPayment.CreditCard
        };

    protected void SetupHttpContext(string userId)
    {
        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, userId) }, "mock"))
        };
        HttpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);
    }

    #endregion
}
