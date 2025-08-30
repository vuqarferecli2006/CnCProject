namespace CnC.Domain.Entities;

public class CurrencyRate:BaseEntity
{
    public string CurrencyCode { get; set; } = default!; 
    public decimal RateAgainstAzn { get; set; } = default!; 
    public DateTime Date { get; set; }
}
