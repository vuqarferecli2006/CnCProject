using CnC.Application.Abstracts.Repositories.ICategoryRepositories;
using CnC.Application.Abstracts.Repositories.ICurrencyRateRepository;
using CnC.Application.Abstracts.Services;
using CnC.Application.DTOs.ElasticSearch;
using CnC.Application.Shared.Responses;
using CnC.Domain.Enums;
using Nest;
using System.Net;

namespace CnC.Infrastructure.Services;

public class ElasticProductService : IElasticProductService
{
    private readonly IElasticClient _client;
    private readonly ICurrencyRateReadRepository _currencyRateReadRepository;
    private readonly ICategoryReadRepository _categoryReadRepository;
    private const string IndexName = "products";

    public ElasticProductService(IElasticClient client, ICurrencyRateReadRepository currencyRateReadRepository,ICategoryReadRepository categoryReadRepository)
    {
        _client = client;
        _currencyRateReadRepository=currencyRateReadRepository;
        _categoryReadRepository=categoryReadRepository;
    }
    public async Task IndexProductAsync(ElasticSearchResponse searchResponse)
    {
        await _client.IndexAsync(searchResponse, idx => idx.Index(IndexName).Id(searchResponse.Id));
    }

    public async Task UpdateProductAsync(ElasticSearchResponse searchResponse)
    {
        await _client.IndexAsync(searchResponse, idx => idx.Index(IndexName).Id(searchResponse.Id));
    }

    public async Task UpdateProductViewCountAsync(Guid productId, int viewCount)
    {
        var response = await _client.UpdateAsync<ElasticSearchResponse, object>(
            productId, u => u
                .Index(IndexName)
                .Doc(new { ViewCount = viewCount })
        );
    }

    public async Task DeleteProductAsync(Guid productId)
    {
        await _client.DeleteAsync<ElasticSearchResponse>(productId, d => d.Index(IndexName));
    }

    public async Task<BaseResponse<List<ElasticSearchResponse>>> SearchAsync(ElasticSearchProductDto dto, CancellationToken cancellationToken)
    {
        var from = (dto.Page - 1) * dto.PageSize;

        var searchResponse = await _client.SearchAsync<ElasticSearchResponse>(s => s
            .Index(IndexName)
            .From(from)
            .Size(dto.PageSize)
            .Query(q =>
            {
                if (!string.IsNullOrWhiteSpace(dto.Query) && dto.CategoryId == null)
                {
                    return q.Wildcard(w => w
                        .Field(p => p.Name)
                        .Value($"*{dto.Query.ToLower()}*")  
                    );
                }

                if (string.IsNullOrWhiteSpace(dto.Query) && dto.CategoryId != null)
                {
                    return q.Term(t => t
                        .Field(p => p.CategoryId.Suffix("keyword"))
                        .Value(dto.CategoryId.ToString())
                    );
                }
                
                return q.Bool(b => b
                    .Must(
                        m => m.Term(t => t
                            .Field(p => p.CategoryId.Suffix("keyword"))
                            .Value(dto.CategoryId.ToString())
                        ),
                        m => m.Match(mm => mm
                            .Field(p => p.Name)
                            .Query(dto.Query)
                            .Fuzziness(Fuzziness.Auto)
                        )
                    )
                );
            })
            .Sort(srt => srt.Descending(p => p.ViewCount))
        , cancellationToken);

        var products = searchResponse.Documents.ToList();

        if (dto.Currency != Currency.AZN)
        {
            var rate = await _currencyRateReadRepository
                .GetCurrencyRateByCodeAsync(dto.Currency.ToString(), cancellationToken);

            if (rate != null)
            {
                foreach (var p in products)
                {
                    p.Price = Math.Round(p.Price / rate.RateAgainstAzn, 1);
                }
            }
        }   

        return new ("Success",products,true,HttpStatusCode.OK);
    }

    public async Task<BaseResponse<List<ElasticSearchResponse>>> FilterProductsAsync(FilterProductDto dto,CancellationToken cancellationToken = default)
    {
        var category=await _categoryReadRepository.GetByIdAsync(dto.CategoryId);
        if(category is null)
            return new("Category not found", HttpStatusCode.NotFound);

        var from = (dto.Page - 1) * dto.PageSize;

        var searchResponse = await _client.SearchAsync<ElasticSearchResponse>(s => s
            .Index(IndexName)
            .From(from)
            .Size(dto.PageSize)
            .Query(q => q.Term(t => t
                .Field(p => p.CategoryId.Suffix("keyword"))
                .Value(dto.CategoryId.ToString())
            ))
            .Sort(srt => GetSort(srt, dto.SortType, dto.SortingForm))
        , cancellationToken);

        var products = searchResponse.Documents.ToList();

        if (dto.Currency != Currency.AZN)
        {
            var rate = await _currencyRateReadRepository
                .GetCurrencyRateByCodeAsync(dto.Currency.ToString(), cancellationToken);

            if (rate != null)
            {
                foreach (var p in products)
                {
                    p.Price = Math.Round(p.Price / rate.RateAgainstAzn, 1);
                }
            }
        }

        return new("Success", products, true, HttpStatusCode.OK);
    }

    private static SortDescriptor<ElasticSearchResponse> GetSort(SortDescriptor<ElasticSearchResponse> sort, SortType sortType, SortingForm sortingForm)
    {
        return sortType switch
        {
            SortType.ViewCount => sortingForm == SortingForm.Descending
                ? sort.Ascending(p => p.ViewCount)
                : sort.Descending(p => p.ViewCount),
            SortType.Score => sortingForm == SortingForm.Descending
                ? sort.Ascending(SortSpecialField.Score)
                : sort.Descending(SortSpecialField.Score),
            SortType.Price => sortingForm == SortingForm.Ascending
                ? sort.Ascending(p => p.Price)
                : sort.Descending(p => p.Price),
            _ => sort.Descending(p => p.ViewCount) 
        };
    }
}
