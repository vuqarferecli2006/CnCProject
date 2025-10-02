using CnC.Application.Abstracts.Repositories.IOrderProductRepositories;
using CnC.Application.Abstracts.Repositories.IOrderRepositories;
using CnC.Application.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace CnC.Application.Features.Order.Commands.RemoveForOrder;

public class RemoveForOrderCommandHandler : IRequestHandler<RemoveForOrderCommandRequest, BaseResponse<string>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IOrderReadRepository _orderReadRepository;
    private readonly IOrderProductWriteRepository _orderProductWriteRepository;
    private readonly IOrderWriteRepository _orderWriteRepository;

    public RemoveForOrderCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IOrderReadRepository orderReadRepository,
        IOrderWriteRepository orderWriteRepository,
        IOrderProductWriteRepository orderProductWriteRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _orderReadRepository = orderReadRepository;
        _orderWriteRepository = orderWriteRepository;
        _orderProductWriteRepository = orderProductWriteRepository;
    }

    public async Task<BaseResponse<string>> Handle(RemoveForOrderCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return new("User not found", HttpStatusCode.Unauthorized);

        var orderProduct = await _orderReadRepository.GetUserActiveOrderProductAsync(userId, request.ProductId, cancellationToken);
        if (orderProduct is null)
            return new("Product not found in orders", HttpStatusCode.NotFound);

        var order = orderProduct.Order;

        order.TotalAmount-=orderProduct.UnitPrice;
        if (order.TotalAmount < 0)
            order.TotalAmount = 0;
        _orderWriteRepository.Update(order);

        _orderProductWriteRepository.Delete(orderProduct);

        await _orderProductWriteRepository.SaveChangeAsync();
        await _orderWriteRepository.SaveChangeAsync();
        return new("Product removed from order", true, HttpStatusCode.OK);
    }
}
