namespace CnC.Application.Shared.Responses;

public class OrderProductResponse
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public string PreviewImageUrl { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}
