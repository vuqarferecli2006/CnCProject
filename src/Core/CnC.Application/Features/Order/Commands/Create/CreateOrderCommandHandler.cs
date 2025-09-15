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
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IBasketReadRepository _basketReadRepository;
    private readonly IProductBasketReadRepository _productBasketReadRepository;
    private readonly IOrderWriteRepository _orderWriteRepository;

    public CreateOrderCommandHandler(IHttpContextAccessor contextAccessor, 
                                    IBasketReadRepository basketReadRepository, 
                                    IProductBasketReadRepository productBasketReadRepository, 
                                    IOrderWriteRepository orderWriteRepository)
    {
        _contextAccessor = contextAccessor;
        _basketReadRepository = basketReadRepository;
        _productBasketReadRepository = productBasketReadRepository;
        _orderWriteRepository = orderWriteRepository;
    }

    public async Task<BaseResponse<string>> Handle(CreateOrderCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return new("User not found", HttpStatusCode.Unauthorized);

        var basket = await _basketReadRepository.GetByBasketUser(userId,cancellationToken);
        if(basket is null)
            return new("Basket not found",HttpStatusCode.NotFound);

        var productBasket=await _productBasketReadRepository.GetByBasketIdAsync(basket.Id,cancellationToken);
        if (productBasket is null)
            return new("Basket is empty", HttpStatusCode.OK);

        var selectedProducts= productBasket.Where(pb => pb.IsSelectedForOrder).ToList();

        if (!selectedProducts.Any())
            return new("No products selected for order", HttpStatusCode.BadRequest);
        
        var order = new Domain.Entities.Order
        {
            UserId = userId,
            OrderDate = DateTime.UtcNow,
            TotalAmount = selectedProducts.Sum(pb => pb.Product.PriceAzn * pb.Quantity),
            OrderProducts = selectedProducts.Select(pb => new OrderProduct
            {
                ProductId = pb.ProductId,
                Quantity = pb.Quantity,
                UnitPrice=pb.Product.PriceAzn,
            }).ToList(),
        };

        await _orderWriteRepository.AddAsync(order);
        await _orderWriteRepository.SaveChangeAsync();

        return new("Created order Successfully", true, HttpStatusCode.Created);
    }
}
