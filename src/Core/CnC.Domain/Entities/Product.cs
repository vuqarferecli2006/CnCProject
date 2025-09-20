namespace CnC.Domain.Entities;

public class Product:BaseEntity
{
    public string Name { get; set; } = null!;

    public string PreviewImageUrl { get; set; } = null!;

    public decimal DiscountedPercent { get; set; }

    public int Score { get; set; }

    public string? Slug { get; set; }

    public decimal PriceAzn { get; set; }

    public Guid CategoryId { get; set; }

    public Category Category { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public AppUser User { get; set; } = null!;

    public ProductDescription ProductDescription { get; set; } = null!;

    public ICollection<ProductCurrency> ProductCurrencies { get; set; } = new List<ProductCurrency>();

    public ICollection<ProductBasket> ProductBaskets { get; set; } = new List<ProductBasket>();

    public ICollection<ProductView> ProductViews { get; set; } = new List<ProductView>();

    public ICollection<BookMark> BookMarks { get; set; } = new List<BookMark>();

    public ICollection<OrderProduct> OrderItems { get; set; } = new List<OrderProduct>();
}
