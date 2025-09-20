using CnC.Application.Abstracts.Repositories.IBasketRepositories;
using CnC.Application.Abstracts.Repositories.ICurrencyRateRepository;
using CnC.Application.Abstracts.Repositories.IProductBasketRepositories;
using CnC.Application.Features.Basket.Queries.GetBasket;
using CnC.Application.Shared.Responses;
using CnC.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

public class GetBasketQueriesHandler : IRequestHandler<GetBasketQueriesRequest, BaseResponse<BasketResponse>>
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IBasketReadRepository _basketReadRepository;
    private readonly IProductBasketReadRepository _productBasketReadRepository;
    private readonly ICurrencyRateReadRepository _currencyRateReadRepository;

    public GetBasketQueriesHandler(
        IHttpContextAccessor contextAccessor,
        IBasketReadRepository basketReadRepository,
        IProductBasketReadRepository productBasketReadRepository,
        ICurrencyRateReadRepository currencyRateReadRepository)
    {
        _contextAccessor = contextAccessor;
        _basketReadRepository = basketReadRepository;
        _productBasketReadRepository = productBasketReadRepository;
        _currencyRateReadRepository = currencyRateReadRepository;
    }

    public async Task<BaseResponse<BasketResponse>> Handle(GetBasketQueriesRequest request, CancellationToken cancellationToken)
    {
        var userId = _contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
            return new("User not found", HttpStatusCode.Unauthorized);

        var basket = await _basketReadRepository.GetByBasketUser(userId, cancellationToken);
        if (basket == null)
            return new("Basket not found", HttpStatusCode.NotFound);

        var productBaskets = await _productBasketReadRepository.GetByBasketIdAsync(basket.Id, cancellationToken);
        if (productBaskets == null || !productBaskets.Any())
            return new("Basket is empty", new BasketResponse(), true, HttpStatusCode.OK);

        decimal rate = 1;
        if (request.Currency != Currency.AZN)
        {
            var currencyRate = await _currencyRateReadRepository.GetCurrencyRateByCodeAsync(request.Currency.ToString(), cancellationToken);
            if (currencyRate == null)
                return new("Currency rate not found", HttpStatusCode.NotFound);

            rate = currencyRate.RateAgainstAzn;
        }

        var basketResponse = new BasketResponse();
        decimal totalBasketPrice = 0;
        basketResponse.BasketId = basket.Id;
        foreach (var pb in productBaskets)
        {
            var product = pb.Product;
            if (product == null || product.ProductDescription == null)
                continue;

            decimal convertedPrice = Math.Round(product.PriceAzn / rate, 1);
            decimal totalPrice = convertedPrice;
            totalBasketPrice += totalPrice;

            basketResponse.Items.Add(new BasketItemResponse
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Model = product.ProductDescription.Model,
                PreviewImageUrl = product.PreviewImageUrl,
                Price = convertedPrice,
            });
        }

        basketResponse.TotalBasketPrice = totalBasketPrice;

        return new("Success", basketResponse, true, HttpStatusCode.OK);
    }
}
