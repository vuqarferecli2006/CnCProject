namespace CnC.Application.Abstracts.Services;

public interface IPaymentStrategy
{
    Task<string> CreatePaymentIntentAsync(decimal amount, string currency);
    Task<string> ConfirmPaymentAsync(string paymentIntentId);
}
