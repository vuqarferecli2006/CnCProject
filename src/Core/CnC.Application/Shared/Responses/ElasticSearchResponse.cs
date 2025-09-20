namespace CnC.Application.Shared.Responses;

public class ElasticSearchResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string PreviewImageUrl { get; set; } = null!;
    public int Score { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountedPercent { get; set; }
    public Guid CategoryId { get; set; }
    public int ViewCount { get; set; }
}
