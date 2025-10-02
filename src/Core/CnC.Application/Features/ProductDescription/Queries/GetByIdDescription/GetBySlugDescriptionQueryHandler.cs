using CnC.Application.Abstracts.Repositories.ICurrencyRateRepository;
using CnC.Application.Abstracts.Repositories.IProductDescriptionRepository;
using CnC.Application.Abstracts.Repositories.IProductRepositories;
using CnC.Application.Abstracts.Repositories.IProductViewRepositories;
using CnC.Application.Abstracts.Services;
using CnC.Application.Shared.Responses;
using CnC.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace CnC.Application.Features.ProductDescription.Queries.GetByIdDescription;

public class GetBySlugDescriptionQueryHandler : IRequestHandler<GetBySlugDescriptionQueryRequest, BaseResponse<ProductDescriptionResponse>>
{
    private readonly IProductDescriptionReadRepository _productDescriptionReadRepository;
    private readonly ICurrencyRateReadRepository _currencyRateReadRepository;
    private readonly IProductWriteRepository _productWriteRepository;
    private readonly IElasticProductService _elasticProductService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProductViewReadRepository _productViewReadRepository;
    private readonly IProductViewWriteRepository _productViewWriteRepository;


    public GetBySlugDescriptionQueryHandler(IProductDescriptionReadRepository productDescriptionReadRepository,
                                        ICurrencyRateReadRepository currencyRateReadRepository,
                                        IProductWriteRepository productWriteRepository,
                                        IElasticProductService elasticProductService,
                                        IProductViewWriteRepository productViewWriteRepository,
                                        IProductViewReadRepository productViewReadRepository,
                                        IHttpContextAccessor httpContextAccessor)
    {
        _productDescriptionReadRepository = productDescriptionReadRepository;
        _currencyRateReadRepository = currencyRateReadRepository;
        _productWriteRepository = productWriteRepository;
        _elasticProductService = elasticProductService;
        _productViewWriteRepository = productViewWriteRepository;
        _httpContextAccessor = httpContextAccessor;
        _productViewReadRepository = productViewReadRepository;
    }

    public async Task<BaseResponse<ProductDescriptionResponse>> Handle(GetBySlugDescriptionQueryRequest request, CancellationToken cancellationToken)
    {
        var productDescription = await _productDescriptionReadRepository
            .GetProductDescriptionBySlugAsync(request.Slug, cancellationToken);

        if (productDescription is null)
            return new("Product not found", HttpStatusCode.NotFound);

        decimal convertedPrice;
        if (request.Currency == Currency.AZN)
            convertedPrice = productDescription.Product.PriceAzn;
        else
        {
            var currencyRate = await _currencyRateReadRepository
                .GetCurrencyRateByCodeAsync(request.Currency.ToString(), cancellationToken);

            if (currencyRate == null)
                return new("Currency rate not found", HttpStatusCode.NotFound);

            convertedPrice = productDescription.Product.PriceAzn / currencyRate.RateAgainstAzn;
        }

        convertedPrice = Math.Round(convertedPrice, 1);

        productDescription.ViewCount++;
        await _productWriteRepository.SaveChangeAsync();
        await _elasticProductService.UpdateProductViewCountAsync(
            productDescription.ProductId, productDescription.ViewCount);

        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!string.IsNullOrEmpty(userId))
        {
            var existingView = await _productViewReadRepository
                .GetSingleAsync(userId,productDescription.ProductId,cancellationToken);

            if (existingView is not null)
            {
                existingView.UpdatedAt = DateTime.UtcNow;
                _productViewWriteRepository.Update(existingView);
            }
            else
            {
                var userViews = await _productViewReadRepository
                    .GetAll().Where(pv => pv.UserId == userId)
                    .OrderBy(pv => pv.UpdatedAt) 
                    .ToListAsync(cancellationToken);

                if (userViews.Count >= 12)
                {
                    var oldestView = userViews.First(); 
                    _productViewWriteRepository.Delete(oldestView);
                }

                var newView = new Domain.Entities.ProductView
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    ProductId = productDescription.ProductId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _productViewWriteRepository.AddAsync(newView);
            }

            await _productViewWriteRepository.SaveChangeAsync();
        }

        var response = new ProductDescriptionResponse
        {
            ProductId = productDescription.ProductId,
            ProductName = productDescription.Product.Name,
            Slug = productDescription.Product.Slug,
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
