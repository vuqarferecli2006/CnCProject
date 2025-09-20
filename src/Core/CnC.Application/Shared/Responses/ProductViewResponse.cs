using CnC.Domain.Enums;

namespace CnC.Application.Shared.Responses;

public class ProductViewResponse
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime ViewedAt { get; set; }
}
