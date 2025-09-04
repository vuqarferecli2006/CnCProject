using CnC.Application.Abstracts.Repositories.ICurrencyRateRepository;
using CnC.Application.Abstracts.Services;
using CnC.Application.DTOs.ElasticSearch;
using CnC.Application.Shared.Responses;
using CnC.Domain.Enums;
using Nest;

namespace CnC.Infrastructure.Services;

public class ElasticProductService : IElasticProductService
{
    private readonly IElasticClient _client;
    private readonly ICurrencyRateReadRepository _currencyRateReadRepository;
    private const string IndexName = "products";

    public ElasticProductService(IElasticClient client, ICurrencyRateReadRepository currencyRateReadRepository)
    {
        _client = client;
        _currencyRateReadRepository=currencyRateReadRepository;
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

    public async Task<List<ElasticSearchResponse>> SearchAsync(ElasticSearchProductDto dto, CancellationToken cancellationToken)
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
                        .Value($"*{dto.Query.ToLower()}*")  // Wildcard ilə substring axtarışı edirik
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
                    p.Price = Math.Round(p.Price / rate.RateAgainstAzn, 2);
                }
            }
        }

        return products;
    }

}
