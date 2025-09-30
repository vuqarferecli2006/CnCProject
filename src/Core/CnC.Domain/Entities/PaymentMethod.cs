using CnC.Domain.Enums;

namespace CnC.Domain.Entities;

public class PaymentMethod:BaseEntity
{
    public string Name { get; set; } = null!;

    public MethodForPayment MethodForPayment { get; set; }

    public string? StripePaymentMethodId { get; set; }

    public string? UserId { get; set; }

    public AppUser? User { get; set; }

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
