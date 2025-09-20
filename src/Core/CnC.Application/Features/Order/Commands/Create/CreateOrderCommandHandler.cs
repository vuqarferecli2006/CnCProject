using CnC.Application.Abstracts.Repositories.IBasketRepositories;
using CnC.Application.Abstracts.Repositories.IOrderRepositories;
using CnC.Application.Abstracts.Repositories.IProductBasketRepositories;
using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace CnC.Application.Features.Order.Commands.Create;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommandRequest, BaseResponse<string>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IBasketReadRepository _basketReadRepository;
    private readonly IProductBasketReadRepository _productBasketReadRepository;
    private readonly IOrderWriteRepository _orderWriteRepository;
    private readonly IOrderReadRepository _orderReadRepository;

    public CreateOrderCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IBasketReadRepository basketReadRepository,
        IProductBasketReadRepository productBasketReadRepository,
        IOrderWriteRepository orderWriteRepository,
        IOrderReadRepository orderReadRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _basketReadRepository = basketReadRepository;
        _productBasketReadRepository = productBasketReadRepository;
        _orderWriteRepository = orderWriteRepository;
        _orderReadRepository = orderReadRepository;
    }

    public async Task<BaseResponse<string>> Handle(CreateOrderCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return new("User not found", HttpStatusCode.Unauthorized);

        var existingOrder = await _orderReadRepository.GetUserActiveOrderAsync(userId, cancellationToken);
        if (existingOrder is not null)
            return new("You already have an active order", HttpStatusCode.BadRequest);

        var basket = await _basketReadRepository.GetByIdAsync(request.BasketId);
        if (basket is null || basket.UserId != userId)
            return new("Basket not found or not belongs to user", HttpStatusCode.NotFound);

        var basketProducts = await _productBasketReadRepository.GetByBasketIdAsync(basket.Id, cancellationToken);
        if (basketProducts is null || !basketProducts.Any())
            return new("Basket is empty", HttpStatusCode.BadRequest);

        var order = new Domain.Entities.Order
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            OrderDate = DateTime.UtcNow,
            isPaid = false,
            TotalAmount = basketProducts.Sum(pb => pb.Product.PriceAzn),
            OrderProducts = basketProducts.Select(pb => new OrderProduct
            {
                Id = Guid.NewGuid(),
                ProductId = pb.ProductId,
                UnitPrice = pb.Product.PriceAzn
            }).ToList()
        };

        await _orderWriteRepository.AddAsync(order);
        await _orderWriteRepository.SaveChangeAsync();

        return new("Order created successfully", true, HttpStatusCode.Created);
    }
}
