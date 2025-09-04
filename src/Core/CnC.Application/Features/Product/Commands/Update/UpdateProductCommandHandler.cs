using CnC.Application.Abstracts.Repositories.ICategoryRepositories;
using CnC.Application.Abstracts.Repositories.IProductCurrencyRepository;
using CnC.Application.Abstracts.Repositories.IProductRepositories;
using CnC.Application.Abstracts.Services;
using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using MediatR;
using System.Net;

namespace CnC.Application.Features.Product.Commands.Update;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, BaseResponse<string>>
{
    private readonly IProductWriteRepository _productWriteRepository;
    private readonly IProductReadRepository _productReadRepository;
    private readonly ICategoryReadRepository _categoryReadRepository;
    private readonly IFileServices _fileService;
    private readonly ICurrencyService _currencyService;
    private readonly IProductCurrencyWriteRepository _productCurrencyWriteRepository;
    private readonly IElasticProductService _elasticProductService;

    public UpdateProductCommandHandler(IProductWriteRepository productWriteRepository,
                                    IProductReadRepository productReadRepository, 
                                    IFileServices fileService,
                                    ICategoryReadRepository categoryReadRepository,
                                    ICurrencyService currencyService,
                                    IProductCurrencyWriteRepository productCurrencyWriteRepository,
                                    IElasticProductService elasticProductService)
    {
        _productWriteRepository = productWriteRepository;
        _productReadRepository = productReadRepository;
        _fileService = fileService;
        _categoryReadRepository = categoryReadRepository;
        _currencyService = currencyService;
        _productCurrencyWriteRepository = productCurrencyWriteRepository;
        _elasticProductService = elasticProductService;
    }

    public async Task<BaseResponse<string>> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
    {
        var productExists= await _productReadRepository.GetByIdWithCurrenciesAsync(request.ProductId,cancellationToken);

        if(productExists is null)
            return new("Product not found", HttpStatusCode.NotFound);

        var categoryExists = await _categoryReadRepository.GetByIdAsync(request.NewCategoryId);

        if (categoryExists is null)
            return new("Category not found", HttpStatusCode.NotFound);

        productExists.Name = request.Name;
        productExists.DiscountedPercent = request.DiscountedPercent;
        productExists.CategoryId = request.NewCategoryId;

        productExists.PriceAzn=request.DiscountedPercent>0
            ? request.PriceAzn * (1 - request.DiscountedPercent / 100m)
            : request.PriceAzn;

        if (request.PreviewImageUrl is not null)
        {
            if (!string.IsNullOrEmpty(productExists.PreviewImageUrl))
                await _fileService.DeleteFileAsync(productExists.PreviewImageUrl);

            var newImagePath = await _fileService.UploadAsync(request.PreviewImageUrl, "product-preview");
            productExists.PreviewImageUrl = newImagePath;
        }

        decimal discountedPriceAzn = request.DiscountedPercent > 0
            ? request.PriceAzn * (1 - request.DiscountedPercent / 100m)
            : request.PriceAzn;


        await UpdateProductCurrenciesAsync(productExists, discountedPriceAzn);


        if (productExists.ProductDescription != null)
            productExists.ProductDescription.DiscountedPercent = productExists.DiscountedPercent;

        _productWriteRepository.Update(productExists);
        await _productWriteRepository.SaveChangeAsync();

        var elasticSearchResponse = new ElasticSearchResponse
        {
            Id=productExists.Id,
            Name=productExists.Name,
            PreviewImageUrl=productExists.PreviewImageUrl,
            Price=productExists.PriceAzn,
            DiscountedPercent=productExists.DiscountedPercent,
            CategoryId=productExists.CategoryId,
        };

        await _elasticProductService.UpdateProductAsync(elasticSearchResponse);

        return new("Product updated successfully", productExists.Name, true, HttpStatusCode.OK);
    }
    private async Task UpdateProductCurrenciesAsync(Domain.Entities.Product product, decimal effectivePrice)
    {
        var currencies = new[] { "USD", "EUR", "TRY" };

        foreach (var code in currencies)
        {
            var rateFromAzn = await _currencyService.ConvertAsync(1, "AZN", code);
            var convertedPrice = Math.Round(effectivePrice * rateFromAzn, 2);

            var currencyRate = await _productReadRepository.GetCurrencyRateByCodeAsync(code);
            if (currencyRate == null) 
                continue;

            var existingCurrency = product.ProductCurrencies
                ?.FirstOrDefault(pc => pc.CurrencyRateId == currencyRate.Id);

            if (existingCurrency != null)
            {
                existingCurrency.ConvertedPrice = convertedPrice;
                existingCurrency.CreatedAt = DateTime.UtcNow.Date;

                _productCurrencyWriteRepository.Update(existingCurrency);
            }
        }

        await _productCurrencyWriteRepository.SaveChangeAsync();
    }
}
