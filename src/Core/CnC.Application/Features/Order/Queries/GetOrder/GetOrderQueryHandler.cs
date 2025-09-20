using CnC.Application.Abstracts.Repositories.IBasketRepositories;
using CnC.Application.Abstracts.Repositories.ICurrencyRateRepository;
using CnC.Application.Abstracts.Repositories.IOrderRepositories;
using CnC.Application.Features.Order.Queries.GetOrder;
using CnC.Application.Shared.Responses;
using CnC.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

public class GetOrderQueryHandler : IRequestHandler<GetOrderQueryRequest, BaseResponse<OrderResponse>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IOrderReadRepository _orderReadRepository;
    private readonly ICurrencyRateReadRepository _currencyRateReadRepository;

    public GetOrderQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        IOrderReadRepository orderReadRepository,
        ICurrencyRateReadRepository currencyRateReadRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _orderReadRepository = orderReadRepository;
        _currencyRateReadRepository = currencyRateReadRepository;
    }

    public async Task<BaseResponse<OrderResponse>> Handle(GetOrderQueryRequest request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return new("User not found", HttpStatusCode.Unauthorized);

        var order = await _orderReadRepository.GetUserActiveOrderAsync(userId, cancellationToken);
        if (order is null)
            return new("No active order found", HttpStatusCode.NotFound);

        if (!order.OrderProducts.Any())
            return new("Product not found in order", HttpStatusCode.NotFound);

        decimal rate = 1;
        if (request.Currency != Currency.AZN)
        {
            var currencyRate = await _currencyRateReadRepository.GetCurrencyRateByCodeAsync(request.Currency.ToString(), cancellationToken);
            if (currencyRate == null)
                return new("Currency rate not found", HttpStatusCode.NotFound);

            rate = currencyRate.RateAgainstAzn;
        }

        var response = new OrderResponse
        {
            OrderId = order.Id,
            OrderDate = order.OrderDate,
            IsPaid = order.isPaid,
            Products = order.OrderProducts.Select(op =>
            {
                decimal convertedUnitPrice = Math.Round(op.Product.PriceAzn / rate, 1);
                decimal convertedTotalPrice = convertedUnitPrice ;

                return new OrderProductResponse
                {
                    ProductId = op.ProductId,
                    ProductName = op.Product?.Name ?? "Unknown Product",
                    Model = op.Product?.ProductDescription?.Model ?? string.Empty,
                    UnitPrice = convertedUnitPrice,
                    PreviewImageUrl = op.Product?.PreviewImageUrl ?? string.Empty
                };
            }).ToList()
        };
        response.TotalOrderPrice = response.Products.Sum(p => p.UnitPrice);

        return new("Success", response, true, HttpStatusCode.OK);
    }
}