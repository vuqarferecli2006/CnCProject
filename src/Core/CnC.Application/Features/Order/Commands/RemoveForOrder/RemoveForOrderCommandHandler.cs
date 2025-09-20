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
    private readonly IOrderProductReadRepository _orderProductReadRepository;
    private readonly IOrderProductWriteRepository _orderProductWriteRepository;

    public RemoveForOrderCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IOrderReadRepository orderReadRepository,
        IOrderProductReadRepository orderProductReadRepository,
        IOrderProductWriteRepository orderProductWriteRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _orderReadRepository = orderReadRepository;
        _orderProductReadRepository = orderProductReadRepository;
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
       
        _orderProductWriteRepository.Delete(orderProduct);
        await _orderProductWriteRepository.SaveChangeAsync();

        return new("Product removed from order", true, HttpStatusCode.OK);
    }
}
