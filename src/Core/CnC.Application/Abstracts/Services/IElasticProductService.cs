using CnC.Application.DTOs.ElasticSearch;
using CnC.Application.Shared.Responses;

namespace CnC.Application.Abstracts.Services;

public interface IElasticProductService
{
    Task IndexProductAsync(ElasticSearchResponse searchResponse);
    Task UpdateProductAsync(ElasticSearchResponse searchResponse);
    Task DeleteProductAsync(Guid productId);
    Task UpdateProductViewCountAsync(Guid productId, int viewCount);
    Task<BaseResponse<List<ElasticSearchResponse>>> SearchAsync(ElasticSearchProductDto dto, CancellationToken cancellationToken);
    Task<BaseResponse<List<ElasticSearchResponse>>> FilterProductsAsync(FilterProductDto dto, CancellationToken cancellationToken);
}
