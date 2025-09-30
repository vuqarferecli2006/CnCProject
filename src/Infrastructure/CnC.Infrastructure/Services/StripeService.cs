using CnC.Application.Abstracts.Services;
using CnC.Application.Shared.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Stripe;

namespace CnC.Infrastructure.Services;

public class StripeService : IStripePaymentService
{
    public StripeService(IConfiguration config)
    {
        StripeConfiguration.ApiKey = config["Stripe:SecretKey"];
    }

    public async Task<string> CreatePaymentIntentAsync(decimal amount, string currency)
    {
        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)(amount * 100),  
            Currency = currency.ToLower(),
            PaymentMethodTypes = new List<string> { "card" }
        };
        var service = new PaymentIntentService();
        var intent = await service.CreateAsync(options);
        return intent.Id;
    }

    public async Task<string> AttachTestCardToIntentAsync(string paymentIntentId)
    {
        var intentService = new PaymentIntentService();
        var updateOptions = new PaymentIntentUpdateOptions
        {
            PaymentMethod = "pm_card_mastercard"
        };
        await intentService.UpdateAsync(paymentIntentId, updateOptions);

        var confirmOptions = new PaymentIntentConfirmOptions
        {
            PaymentMethod = "pm_card_mastercard"  
        };
        var confirmed = await intentService.ConfirmAsync(paymentIntentId, confirmOptions);

        return confirmed.Status;
    }

}