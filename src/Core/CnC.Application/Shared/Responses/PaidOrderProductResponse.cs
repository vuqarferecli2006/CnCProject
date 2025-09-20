namespace CnC.Application.Shared.Responses;

public class PaidOrderProductResponse
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public string Model { get; set; } = null!;  
    public decimal TotalPrice { get; set; }
}
