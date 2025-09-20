using System.Numerics;

namespace CnC.Application.Shared.Responses;

public class GetPaidOrderResponse
{
    public Guid OrderId { get; set; }
    public string Currency { get; set; } = null!;
    public bool isPaid { get; set; }
    public List<PaidOrderProductResponse> Product = new();
    public decimal TotalOrderAmount { get; set; }
}
