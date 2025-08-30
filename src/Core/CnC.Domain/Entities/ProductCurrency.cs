namespace CnC.Domain.Entities;

public class ProductCurrency:BaseEntity
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public Guid CurrencyRateId { get; set; }
    public CurrencyRate CurrencyRate { get; set; } = null!;

    public decimal ConvertedPrice { get; set; }
}
