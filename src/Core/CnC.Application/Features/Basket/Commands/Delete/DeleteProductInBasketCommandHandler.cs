using CnC.Application.Abstracts.Repositories.IBasketRepositories;
using CnC.Application.Abstracts.Repositories.IOrderRepositories;
using CnC.Application.Abstracts.Repositories.IProductBasketRepositories;
using CnC.Application.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace CnC.Application.Features.Basket.Commands.Delete;

public class DeleteProductInBasketCommandHandler : IRequestHandler<DeleteProductInBasketCommandRequest, BaseResponse<string>>
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IBasketReadRepository _basketReadRepository;
    private readonly IProductBasketReadRepository _productBasketReadRepository;
    private readonly IProductBasketWriteRepository _productBasketWriteRepository;
    private readonly IOrderReadRepository _orderReadRepository;
    private readonly IOrderWriteRepository _orderWriteRepository;

    public DeleteProductInBasketCommandHandler(
        IHttpContextAccessor contextAccessor,
        IBasketReadRepository basketReadRepository,
        IProductBasketReadRepository productBasketReadRepository,
        IProductBasketWriteRepository productBasketWriteRepository,
        IOrderReadRepository orderReadRepository,
        IOrderWriteRepository orderWriteRepository)
    {
        _contextAccessor = contextAccessor;
        _basketReadRepository = basketReadRepository;
        _productBasketReadRepository = productBasketReadRepository;
        _productBasketWriteRepository = productBasketWriteRepository;
        _orderReadRepository = orderReadRepository;
        _orderWriteRepository = orderWriteRepository;
    }

    public async Task<BaseResponse<string>> Handle(DeleteProductInBasketCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return new("User not found", HttpStatusCode.Unauthorized);

        var basket = await _basketReadRepository.GetByBasketUser(userId, cancellationToken);
        if (basket is null)
            return new("Basket not found", HttpStatusCode.NotFound);

        var existProduct = await _productBasketReadRepository.ExistProductInBasket(basket.Id, request.ProductId, cancellationToken);
        if (existProduct is null)
            return new("Product not found in basket", HttpStatusCode.NotFound);

        _productBasketWriteRepository.Delete(existProduct);
        await _productBasketWriteRepository.SaveChangeAsync();

        var order = await _orderReadRepository.GetUserActiveOrderAsync(userId, cancellationToken);
        if (order != null)
        {
            var orderProduct = order.OrderProducts.FirstOrDefault(op => op.ProductId == request.ProductId);
            if (orderProduct != null)
            {
                order.OrderProducts.Remove(orderProduct);
                order.TotalAmount = order.OrderProducts.Sum(op => op.UnitPrice);
                await _orderWriteRepository.SaveChangeAsync();
            }
        }
        return new("Removed successfully", true, HttpStatusCode.OK);
    }
}

