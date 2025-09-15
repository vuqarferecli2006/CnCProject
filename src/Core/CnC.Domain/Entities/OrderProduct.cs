namespace CnC.Domain.Entities;

public class OrderProduct:BaseEntity
{
    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }  

    public Guid OrderId { get; set; }

    public Order Order { get; set; } = null!;

    public Guid ProductId { get; set; }

    public Product Product { get; set; } = null!;

    public ICollection<Download> Downloads { get; set; } = new List<Download>();
}

