namespace CnC.Application.Shared.Responses;

public class BasketResponse
{
    public List<BasketItemResponse> Items { get; set; } = new List<BasketItemResponse>();
    public decimal TotalBasketPrice { get; set; }
}
