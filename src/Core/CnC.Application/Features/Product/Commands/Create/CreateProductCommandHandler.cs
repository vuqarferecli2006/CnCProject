using CnC.Application.Abstracts.Repositories.IProductCurrencyRepository;
using CnC.Application.Abstracts.Repositories.IProductRepositories;
using CnC.Application.Abstracts.Services;
using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using CnC.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Security.Claims;

namespace CnC.Application.Features.Product.Commands.Create;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, BaseResponse<string>>
{
    private readonly IProductReadRepository _productReadRepository;
    private readonly IProductWriteRepository _productWriteRepository;
    private readonly IFileServices _fileService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICurrencyService _currencyService;
    private readonly IProductCurrencyWriteRepository _productCurrencyWriteRepository;
    private readonly IProductCurrencyReadRepository _productCurrencyReadRepository;
    private readonly IElasticProductService _elasticProductService;

    public CreateProductCommandHandler(IProductReadRepository productReadRepository,
                                    IProductWriteRepository productWriteRepository,
                                    IFileServices fileServices,
                                    IHttpContextAccessor httpContextAccessor,
                                    ICurrencyService currencyService,
                                    IProductCurrencyWriteRepository productCurrencyWriteRepository,
                                    IProductCurrencyReadRepository productCurrencyReadRepository,
                                    IElasticProductService elasticProductService)
    {
        _productReadRepository = productReadRepository;
        _productWriteRepository = productWriteRepository;
        _fileService = fileServices;
        _httpContextAccessor = httpContextAccessor;
        _currencyService = currencyService;
        _productCurrencyWriteRepository = productCurrencyWriteRepository;
        _productCurrencyReadRepository = productCurrencyReadRepository;
        _elasticProductService = elasticProductService;
    }

    public async Task<BaseResponse<string>> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return new("User not found", HttpStatusCode.Unauthorized);

        var previewUrl = await _fileService.UploadAsync(request.PreviewImageUrl, "product-preview");

        var existingProduct = await _productReadRepository.GetAll()
            .Where(p => p.Name.ToLower().Trim() == request.Name.ToLower().Trim())
            .ToListAsync();

        if (existingProduct.Any())
            return new("A product with the same name already exists.", HttpStatusCode.BadRequest);

        decimal priceToSet = request.DiscountedPercent > 0
            ? request.PriceAzn * (1 - request.DiscountedPercent / 100m)
            : request.PriceAzn; 

        var product = new Domain.Entities.Product
        {
            Name = request.Name,
            CategoryId = request.CategoryId,
            PreviewImageUrl = previewUrl,
            DiscountedPercent = request.DiscountedPercent,
            UserId = userId,
            PriceAzn = priceToSet, 
        };

        await _productWriteRepository.AddAsync(product);
        await _productWriteRepository.SaveChangeAsync();

        await UpdateProductCurrenciesAsync(product);

        var elasticSearchRespone = new ElasticSearchResponse
        {
            Id = product.Id,
            Name=product.Name,
            PreviewImageUrl=product.PreviewImageUrl,
            Price=product.PriceAzn,
            DiscountedPercent=product.DiscountedPercent,
            CategoryId=product.CategoryId
        };

        await _elasticProductService.IndexProductAsync(elasticSearchRespone);

        return new("Product created successfully", product.Name, true, HttpStatusCode.Created);
    }

    private async Task UpdateProductCurrenciesAsync(Domain.Entities.Product product)
    {
        var existingCurrencies = await _productCurrencyReadRepository.GetAll()
            .Where(pc => pc.ProductId == product.Id)
            .ToListAsync();

        if (existingCurrencies.Any())
        {
            _productCurrencyWriteRepository.DeleteRange(existingCurrencies);
            await _productCurrencyWriteRepository.SaveChangeAsync();
        }

        var currencies = new[] { "USD", "EUR", "TRY" };

        foreach (var code in currencies)
        {
            var rateToAzn = await _currencyService.ConvertAsync(1, code, "AZN");

            var convertedPrice = Math.Round(product.PriceAzn / rateToAzn, 1); 

            var currencyRate = await _productReadRepository.GetCurrencyRateByCodeAsync(code);

            if (currencyRate != null)
            {
                var productCurrency = new ProductCurrency
                {
                    ProductId = product.Id,
                    CurrencyRateId = currencyRate.Id,
                    ConvertedPrice = convertedPrice,
                    CreatedAt = DateTime.UtcNow
                };

                await _productCurrencyWriteRepository.AddAsync(productCurrency);
            }
        }

        await _productCurrencyWriteRepository.SaveChangeAsync();
    }
}
