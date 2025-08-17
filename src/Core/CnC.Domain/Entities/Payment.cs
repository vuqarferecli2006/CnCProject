namespace CnC.Domain.Entities;

public class Payment: BaseEntity
{
    public Guid OrderId { get; set; }

    public Order Order { get; set; } = null!;

    public Guid PaymentMethodId { get; set; }

    public PaymentMethod PaymentMethod { get; set; } = null!;
}
