namespace CnC.Domain.Entities;

public class ProductBasket:BaseEntity
{
    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }

    public string PreviewImageUrl { get; set; } = null!;

    public Guid ProductId { get; set; }

    public Product Product { get; set; } = null!;

    public Guid BasketId { get; set; }

    public Basket Basket { get; set; } = null!;
}
