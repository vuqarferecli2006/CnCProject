using CnC.Domain.Enums;

namespace CnC.Application.DTOs.ElasticSearch;

public class FilterProductDto
{
    public Guid CategoryId { get; set; }

    public SortingForm SortingForm { get; set; }

    public SortType SortType { get; set; }

    public Currency Currency { get; set; } = Currency.AZN;

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}
