using CnC.Application.Abstracts.Repositories.ICurrencyRateRepository;
using CnC.Application.Abstracts.Repositories.IProductCurrencyRepository;
using CnC.Application.Abstracts.Repositories.IProductDescriptionRepository;
using CnC.Application.Abstracts.Repositories.IProductRepositories;
using CnC.Application.Abstracts.Services;
using CnC.Application.Shared.Responses;
using CnC.Domain.Enums;
using MediatR;
using System.Net;

namespace CnC.Application.Features.ProductDescription.Queries.GetByIdDescription;

public class GetProductDescriptionQueryHandler : IRequestHandler<GetByIdDescriptionQueryRequest, BaseResponse<ProductDescriptionResponse>>
{
    private readonly IProductDescriptionReadRepository _productDescriptionReadRepository;
    private readonly ICurrencyRateReadRepository _currencyRateReadRepository;
    private readonly IProductWriteRepository _productWriteRepository;
    private readonly IElasticProductService _elasticProductService;

    public GetProductDescriptionQueryHandler(IProductDescriptionReadRepository productDescriptionReadRepository,
                                        ICurrencyRateReadRepository currencyRateReadRepository,
                                        IProductWriteRepository productWriteRepository,
                                        IElasticProductService elasticProductService)
    {
        _productDescriptionReadRepository = productDescriptionReadRepository;
        _currencyRateReadRepository = currencyRateReadRepository;
        _productWriteRepository = productWriteRepository;
        _elasticProductService = elasticProductService;
    }

    public async Task<BaseResponse<ProductDescriptionResponse>> Handle(GetByIdDescriptionQueryRequest request, CancellationToken cancellationToken)
    {
        var productDescription = await _productDescriptionReadRepository.GetProductDescriptionByIdAsync(request.ProductId, cancellationToken);

        if (productDescription is null)
            return new("Product not found", HttpStatusCode.NotFound);
        
        decimal convertedPrice;

        if (request.Currency == Currency.AZN)
            convertedPrice = productDescription.Product.PriceAzn;
        else
        {
            var currencyRate = await _currencyRateReadRepository.GetCurrencyRateByCodeAsync(request.Currency.ToString(), cancellationToken);

            if (currencyRate == null)
                return new("Currency rate not found", HttpStatusCode.NotFound);
            
            convertedPrice = productDescription.Product.PriceAzn / currencyRate.RateAgainstAzn;
        }

        convertedPrice = Math.Round(convertedPrice, 2);

        productDescription.ViewCount++;

        await _productWriteRepository.SaveChangeAsync();

        await _elasticProductService.UpdateProductViewCountAsync(productDescription.ProductId, productDescription.ViewCount);

        var response = new ProductDescriptionResponse
        {
            ProductId = productDescription.ProductId,
            ProductName = productDescription.Product.Name,
            Description = productDescription.Description,
            Model = productDescription.Model,
            ImageUrl = productDescription.ImageUrl,
            ViewCount = productDescription.ViewCount,
            DiscountedPercent = productDescription.DiscountedPercent,
            Score = productDescription.Score,
            Price = convertedPrice
        };

        return new("Success", response, true, HttpStatusCode.OK);
    }
}
