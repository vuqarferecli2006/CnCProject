using CnC.Domain.Enums;

namespace CnC.Application.DTOs.ElasticSearch;

public record ElasticSearchProductDto
{
    public string? Query { get; set; }
    public Guid? CategoryId { get; set; }
    public Currency Currency { get; set; } = Currency.AZN;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
