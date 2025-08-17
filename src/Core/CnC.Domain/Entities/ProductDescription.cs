namespace CnC.Domain.Entities;

public class ProductDescription:BaseEntity
{
    public string ImageUrl { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int ViewCount { get; set; }

    public decimal DiscountedPercent { get; set; }

    public int Score { get; set; }

    public string Model { get; set; } = null!;

    public Guid ProductId { get; set; }

    public Product Product { get; set; } = null!;
}
