using CnC.Application.Abstracts.Repositories.IBasketRepositories;
using CnC.Application.Abstracts.Repositories.IOrderProductRepositories;
using CnC.Application.Abstracts.Repositories.IOrderRepositories;
using CnC.Application.Abstracts.Repositories.IProductBasketRepositories;
using CnC.Application.Abstracts.Repositories.IProductRepositories;
using CnC.Application.Abstracts.Services;
using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace CnC.Application.Features.Basket.Commands.Create;

public class AddBasketCommandHandler : IRequestHandler<AddBasketCommandRequest, BaseResponse<string>>
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IBasketReadRepository _basketReadRepository;
    private readonly IBasketWriteRepository _basketWriteRepository;
    private readonly IProductBasketReadRepository _productBasketReadRepository;
    private readonly IProductBasketWriteRepository _productBasketWriteRepository;
    private readonly IProductReadRepository _productReadRepository;
    private readonly IProductWriteRepository _productWriteRepository;
    private readonly IElasticProductService _elasticProductService;
    private readonly IOrderReadRepository _orderReadRepository;
    private readonly IOrderWriteRepository _orderWriteRepository;
    private readonly IOrderProductWriteRepository _orderProductWriteRepository;

    public AddBasketCommandHandler(
        IHttpContextAccessor contextAccessor,
        IBasketReadRepository basketReadRepository,
        IBasketWriteRepository basketWriteRepository,
        IProductBasketReadRepository productBasketReadRepository,
        IProductBasketWriteRepository productBasketWriteRepository,
        IProductReadRepository productReadRepository,
        IProductWriteRepository productWriteRepository,
        IElasticProductService elasticProductService,
        IOrderReadRepository orderReadRepository,
        IOrderProductWriteRepository orderProductWriteRepository,
        IOrderWriteRepository orderWriteRepository)
    {
        _contextAccessor = contextAccessor;
        _basketReadRepository = basketReadRepository;
        _basketWriteRepository = basketWriteRepository;
        _productBasketReadRepository = productBasketReadRepository;
        _productBasketWriteRepository = productBasketWriteRepository;
        _productReadRepository = productReadRepository;
        _productWriteRepository = productWriteRepository;
        _elasticProductService = elasticProductService;
        _orderReadRepository = orderReadRepository;
        _orderWriteRepository = orderWriteRepository;
        _orderProductWriteRepository = orderProductWriteRepository;
    }

    public async Task<BaseResponse<string>> Handle(AddBasketCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return new("User not found", HttpStatusCode.Unauthorized);

        var basket = await _basketReadRepository.GetByBasketUser(userId, cancellationToken);
        if (basket is null)
        {
            basket = new Domain.Entities.Basket
            {
                UserId = userId
            };
            await _basketWriteRepository.AddAsync(basket);
            await _basketWriteRepository.SaveChangeAsync();
        }

        var product = await _productReadRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
            return new("Product not found", HttpStatusCode.NotFound);

        var existingProduct = await _productBasketReadRepository.ExistProductInBasket(basket.Id, request.ProductId, cancellationToken);
        if (existingProduct is not null)
            return new("Productalready exist", HttpStatusCode.OK);


        var productBasket = new ProductBasket
        {
            Id = Guid.NewGuid(),
            BasketId = basket.Id,
            ProductId = request.ProductId,
            IsSelectedForOrder = true,
            TotalPrice = product.PriceAzn,
            PreviewImageUrl = product.PreviewImageUrl,
        };
        await _productBasketWriteRepository.AddAsync(productBasket);

        await _productBasketWriteRepository.SaveChangeAsync();

        var order = await _orderReadRepository.GetUserActiveOrderAsync(userId, cancellationToken);
        if (order is not null)
        {
            var existingOrderProduct = order.OrderProducts.FirstOrDefault(op => op.ProductId == request.ProductId);
            var basketProduct = await _productBasketReadRepository.ExistProductInBasket(basket.Id, request.ProductId, cancellationToken);

            if (existingOrderProduct is not null)
                existingOrderProduct.UnitPrice = product.PriceAzn;

            var orderProduct = new OrderProduct
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                ProductId = product.Id,
                UnitPrice = product.PriceAzn
            };
            await _orderProductWriteRepository.AddAsync(orderProduct);
           
            order.TotalAmount = order.OrderProducts.Sum(op => op.UnitPrice);

            await _orderProductWriteRepository.SaveChangeAsync();
            await _orderWriteRepository.SaveChangeAsync();
        }

        product.Score += 5;
        _productWriteRepository.Update(product);
        await _productWriteRepository.SaveChangeAsync();
        await _elasticProductService.UpdateProductScoreAsync(product.Id, product.Score);

        return new("Product successfully added to basket", true, HttpStatusCode.OK);
    }
}
