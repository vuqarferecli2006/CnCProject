namespace CnC.Application.Shared.Responses;

public class OrderResponse
{
    public Guid OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsPaid { get; set; }
    public List<OrderProductResponse> Products { get; set; } = new();
    public decimal TotalOrderPrice { get; set; }
}
