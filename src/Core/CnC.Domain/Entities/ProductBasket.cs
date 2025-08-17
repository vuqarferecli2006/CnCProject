namespace CnC.Domain.Entities;

public class ProductBasket
{
    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }

    public Guid ProductId { get; set; }

    public Product Product { get; set; } = null!;

    public Guid BasketId { get; set; }

    public Basket Basket { get; set; } = null!;
}
