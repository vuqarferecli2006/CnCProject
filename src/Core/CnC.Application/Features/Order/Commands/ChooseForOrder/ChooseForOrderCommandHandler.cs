using CnC.Application.Abstracts.Repositories.IBasketRepositories;
using CnC.Application.Abstracts.Repositories.IOrderProductRepositories;
using CnC.Application.Abstracts.Repositories.IOrderRepositories;
using CnC.Application.Abstracts.Repositories.IProductBasketRepositories;
using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace CnC.Application.Features.Order.Commands.ChooseForOrder;

public class ChooseForOrderCommandHandler : IRequestHandler<ChooseForOrderCommandRequest, BaseResponse<string>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IBasketReadRepository _basketReadRepository;
    private readonly IProductBasketReadRepository _productBasketReadRepository;
    private readonly IOrderWriteRepository _orderWriteRepository;
    private readonly IOrderProductWriteRepository _orderProductWriteRepository;
    private readonly IProductBasketWriteRepository _productBasketWriteRepository;
    private readonly IOrderReadRepository _orderReadRepository;
    public ChooseForOrderCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IBasketReadRepository basketReadRepository,
        IProductBasketReadRepository productBasketReadRepository,
        IOrderWriteRepository orderWriteRepository,
        IOrderProductWriteRepository orderProductWriteRepository,
        IProductBasketWriteRepository productBasketWriteRepository,
        IOrderReadRepository orderReadRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _basketReadRepository = basketReadRepository;
        _productBasketReadRepository = productBasketReadRepository;
        _orderWriteRepository = orderWriteRepository;
        _orderProductWriteRepository = orderProductWriteRepository;
        _productBasketWriteRepository = productBasketWriteRepository;
        _orderReadRepository = orderReadRepository;
    }

    public async Task<BaseResponse<string>> Handle(ChooseForOrderCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return new("User not found", HttpStatusCode.Unauthorized);

        // Basketi yoxla
        var basket = await _basketReadRepository.GetByBasketUser(userId, cancellationToken);
        if (basket == null)
            return new("Basket not found", HttpStatusCode.NotFound);

        var productBasket = await _productBasketReadRepository.ExistProductInBasket(basket.Id, request.ProductId, cancellationToken);
        if (productBasket == null)
            return new("Product not in basket", HttpStatusCode.NotFound);

        productBasket.IsSelectedForOrder = true;
        _productBasketWriteRepository.Update(productBasket);
        await _productBasketWriteRepository.SaveChangeAsync();

        var order = await _orderReadRepository.GetUserActiveOrderAsync(userId, cancellationToken);
        if (order == null)
        {
            order = new Domain.Entities.Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = 0,
                isPaid = false
            };
            await _orderWriteRepository.AddAsync(order);
            await _orderWriteRepository.SaveChangeAsync();
        }

        var existingOrderProduct = order.OrderProducts.FirstOrDefault(op => op.ProductId == productBasket.ProductId);
        if (existingOrderProduct is not null)
            return new("Already added", HttpStatusCode.BadRequest);

        var orderProduct = new OrderProduct
        {
            ProductId = productBasket.ProductId,
            OrderId = order.Id,
            UnitPrice = productBasket.Product.PriceAzn
        };
        await _orderProductWriteRepository.AddAsync(orderProduct);
        await _orderProductWriteRepository.SaveChangeAsync();


        return new("Product selected for order", true, HttpStatusCode.OK);
    }
}
