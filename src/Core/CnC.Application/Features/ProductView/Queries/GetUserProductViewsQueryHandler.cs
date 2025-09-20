using CnC.Application.Abstracts.Repositories.ICurrencyRateRepository;
using CnC.Application.Abstracts.Repositories.IProductViewRepositories;
using CnC.Application.Shared.Helpers.ContextHelper;
using CnC.Application.Shared.Responses;
using CnC.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace CnC.Application.Features.ProductView.Queries;

public class GetUserProductViewsQueryHandler : IRequestHandler<GetUserProductViewsQueryRequest, BaseResponse<List<ProductViewResponse>>>
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IProductViewReadRepository _productViewReadRepository;
    private readonly ICurrencyRateReadRepository _currencyRateReadRepository;
    
    public GetUserProductViewsQueryHandler(IHttpContextAccessor contextAccessor,
                                           IProductViewReadRepository productViewReadRepository,
                                           ICurrencyRateReadRepository currencyRateReadRepository)
    {
        _contextAccessor = contextAccessor;
        _productViewReadRepository = productViewReadRepository;
        _currencyRateReadRepository = currencyRateReadRepository;
    }

    public async Task<BaseResponse<List<ProductViewResponse>>> Handle(GetUserProductViewsQueryRequest request, CancellationToken cancellationToken)
    {
        var userId = _contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        string? sessionId = null;

        if (string.IsNullOrEmpty(userId))
            sessionId = _contextAccessor.HttpContext.ResolveSessionId();

        if (userId is null && sessionId is null)
            return new("Unauthorized", HttpStatusCode.Unauthorized);

        var views = await _productViewReadRepository.GetUserViewsAsync(userId, sessionId, cancellationToken);

        var response = new List<ProductViewResponse>();

        foreach (var view in views)
        {
            decimal price;

            if (request.Currency == Currency.AZN)
            {
                price = view.Product.PriceAzn;
            }
            else
            {
                var currencyRate = await _currencyRateReadRepository
                    .GetCurrencyRateByCodeAsync(request.Currency.ToString(), cancellationToken);

                if (currencyRate == null)
                    return new("Currency rate not found", HttpStatusCode.NotFound);

                price = Math.Round(view.Product.PriceAzn / currencyRate.RateAgainstAzn, 1);
            }

            response.Add(new ProductViewResponse
            {
                ProductId = view.ProductId,
                ProductName = view.Product.Name,
                ImageUrl = view.Product.PreviewImageUrl ?? string.Empty,
                Price = price,
                ViewedAt = view.ViewedAt
            });
        }

        return new("Success", response, true, HttpStatusCode.OK);
    }
}
