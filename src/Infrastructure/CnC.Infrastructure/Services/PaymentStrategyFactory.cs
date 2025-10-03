using CnC.Application.Abstracts.Services;
using CnC.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace CnC.Infrastructure.Services;

public class PaymentStrategyFactory : IPaymentStrategyFactory
{
    private readonly IServiceProvider _serviceProvider;

    public PaymentStrategyFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IPaymentStrategy GetPaymentStrategy(MethodForPayment method)
    {
        return method switch
        {
            MethodForPayment.CreditCard => _serviceProvider.GetRequiredService<StripePaymentStrategy>(),
            _ => throw new NotSupportedException("Unsupported payment method")
        };
    }
}
