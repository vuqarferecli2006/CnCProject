using CnC.Domain.Enums;

namespace CnC.Domain.Entities;

public class Payment: BaseEntity
{
    public Guid OrderId { get; set; }

    public Order Order { get; set; } = null!;
    
    public Currency Currency { get; set; }

    public PaymentStatus Status { get; set; }=PaymentStatus.Pending;

    public decimal Amount { get; set; }

    public string PaymentIntentId { get; set; } = null!;

    public Guid PaymentMethodId { get; set; }

    public PaymentMethod PaymentMethod { get; set; } = null!;
}
