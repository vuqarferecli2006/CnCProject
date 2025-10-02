namespace CnC.Application.Abstracts.Services;

public interface IStripePaymentService
{
    Task<string> CreatePaymentIntentAsync(decimal amount, string currency);
    Task<string> AttachTestCardToIntentAsync(string paymentIntentId);
}
