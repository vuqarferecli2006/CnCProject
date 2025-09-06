namespace CnC.Application.Shared.Responses;

public class FavouriteResponse
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public string PreviewImageUrl { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal DiscountedPercent { get; set; }
    public Guid CategoryId { get; set; }
}
