using CnC.Domain.Enums;

namespace CnC.Application.Shared.Responses;

public class BasketItemResponse
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string PreviewImageUrl { get; set; } = null!;
    public decimal Price { get; set; }
}
