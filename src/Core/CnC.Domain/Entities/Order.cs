namespace CnC.Domain.Entities;

public class Order:BaseEntity
{
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public decimal TotalAmount { get; set; }

    public bool isPaid { get; set; }=false;

    public string UserId { get; set; } = null!;

    public AppUser User { get; set; } = null!;

    public Payment Payment { get; set; } = null!;

    public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
}
