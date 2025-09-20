using CnC.Application.Abstracts.Repositories.ICurrencyRateRepository;
using CnC.Application.Abstracts.Repositories.IFavouriteRepositories;
using CnC.Application.Abstracts.Repositories.IProductRepositories;
using CnC.Application.Shared.Responses;
using CnC.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace CnC.Application.Features.Favourite.Queries;

public class GetAllFavouriteQueriesHandler : IRequestHandler<GetAllFavouriteQuerisRequest, BaseResponse<List<FavouriteResponse>>>
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IFavouriteReadRepository _favouriteReadRepository;
    private readonly IProductReadRepository _productReadRepository;
    private readonly ICurrencyRateReadRepository _currencyRateReadRepository;

    public GetAllFavouriteQueriesHandler(IHttpContextAccessor contextAccessor,
                                         IFavouriteReadRepository favouriteReadRepository,
                                         IProductReadRepository productReadRepository,
                                         ICurrencyRateReadRepository currencyRateReadRepository)
    {
        _contextAccessor = contextAccessor;
        _favouriteReadRepository = favouriteReadRepository;
        _productReadRepository = productReadRepository;
        _currencyRateReadRepository = currencyRateReadRepository;
    }

    public async Task<BaseResponse<List<FavouriteResponse>>> Handle(GetAllFavouriteQuerisRequest request, CancellationToken cancellationToken)
    {
        var userId = _contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
            return new("User not found", HttpStatusCode.Unauthorized);

        var favourites = await _favouriteReadRepository.GetUserFavouritesAsync(userId, cancellationToken);

        if (favourites is null || !favourites.Any())
            return new("No favourites found",HttpStatusCode.OK);

        decimal? rate=null;
        if (request.Currency != Currency.AZN)
        {
            var currencyRate = await _currencyRateReadRepository.GetCurrencyRateByCodeAsync(request.Currency.ToString(), cancellationToken);
            if (currencyRate == null)
                return new("Currency rate not found", HttpStatusCode.NotFound);

             rate = currencyRate.RateAgainstAzn;
        }

        var responses = new List<FavouriteResponse>();

        foreach (var fav in favourites)
        {
            var product = await _productReadRepository.GetByIdAsync(fav.ProductId, cancellationToken);
            if (product == null)
                continue;

            decimal convertedPrice = request.Currency == Currency.AZN
                ? product.PriceAzn
                : product.PriceAzn / rate!.Value;

            convertedPrice = Math.Round(convertedPrice, 1);

            responses.Add(new FavouriteResponse
            {
                ProductId = product.Id,
                ProductName = product.Name,
                PreviewImageUrl = product.PreviewImageUrl,
                Price = convertedPrice,
                DiscountedPercent = product.DiscountedPercent,
                CategoryId = product.CategoryId
            });
        }

        return new("Success", responses, true, HttpStatusCode.OK);
    }
}
