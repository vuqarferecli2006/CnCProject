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
        if (order == null)
            return new("No active order found", HttpStatusCode.NotFound);

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
                decimal convertedUnitPrice = Math.Round(op.UnitPrice / rate, 1);
                decimal convertedTotalPrice = convertedUnitPrice * op.Quantity;

                return new OrderProductResponse
                {
                    ProductId = op.ProductId,
                    ProductName = op.Product.Name,
                    Quantity = op.Quantity,
                    UnitPrice = convertedUnitPrice,
                    TotalPrice = convertedTotalPrice,
                    PreviewImageUrl = op.Product.PreviewImageUrl
                };
            }).ToList()
        };

        response.TotalOrderPrice = response.Products.Sum(p => p.TotalPrice);

        return new("Success", response, true, HttpStatusCode.OK);
    }
}