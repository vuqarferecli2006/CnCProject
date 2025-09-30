using CnC.Application.Abstracts.Repositories.IOrderRepositories;
using CnC.Application.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace CnC.Application.Features.Order.Queries.GetPaidOrder;

public class GetPaidOrderQueryHandler : IRequestHandler<GetPaidOrderQueryRequest, BaseResponse<List<GetPaidOrderResponse>>>
{
    private readonly IOrderReadRepository _orderReadRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetPaidOrderQueryHandler(IOrderReadRepository orderReadRepository, IHttpContextAccessor httpContextAccessor)
    {
        _orderReadRepository = orderReadRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<BaseResponse<List<GetPaidOrderResponse>>> Handle(GetPaidOrderQueryRequest request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return new("User not found", HttpStatusCode.Unauthorized);

        var orders = await _orderReadRepository.GetPaidOrdersByUserIdAsync(userId, cancellationToken);

        if (!orders.Any())
            return new("No paid orders found", HttpStatusCode.NotFound);

        var response = orders.Select(order =>
        {
            var payment = order.Payment;

            return new GetPaidOrderResponse
            {
                OrderId = order.Id,
                Currency = payment?.Currency.ToString() ?? "AZN",
                isPaid = order.isPaid,
                TotalOrderAmount = order.TotalAmount,
                Product = order.OrderProducts.Select(op => new PaidOrderProductResponse
                {
                    ProductId = op.ProductId,
                    ProductName = op.Product?.Name ?? "",
                    Model = op.Product?.ProductDescription?.Model ?? "",
                    TotalPrice = op.UnitPrice
                }).ToList()
            };
        }).ToList();

        return new("Success", response, true, HttpStatusCode.OK);
    }
}

