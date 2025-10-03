using CnC.Domain.Enums;

namespace CnC.Application.Abstracts.Services;

public interface IPaymentStrategyFactory
{
    IPaymentStrategy GetPaymentStrategy(MethodForPayment method);
}
