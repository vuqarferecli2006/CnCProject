using System.Security.Cryptography.X509Certificates;

namespace CnC.Application.Shared.Responses;

public class ProductDescriptionResponse
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public string? Slug { get; set; }
    public string Description { get; set; } = null!; 
    public string Model { get; set; } = null!; 
    public string ImageUrl { get; set; } = null!; 
    public int ViewCount { get; set; }
    public decimal DiscountedPercent { get; set; } 
    public int Score { get; set; } 
    public decimal Price { get; set; } 
}
