using CnC.Application.Abstracts.Repositories.IBasketRepositories;
using CnC.Application.Abstracts.Repositories.IProductBasketRepositories;
using CnC.Application.Abstracts.Repositories.IProductRepositories;
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

    public AddBasketCommandHandler(IHttpContextAccessor contextAccessor, 
                                IBasketReadRepository basketReadRepository, 
                                IBasketWriteRepository basketWriteRepository, 
                                IProductBasketReadRepository productBasketReadRepository, 
                                IProductBasketWriteRepository productBasketWriteRepository,
                                IProductReadRepository productReadRepository)
    {
        _contextAccessor = contextAccessor;
        _basketReadRepository = basketReadRepository;
        _basketWriteRepository = basketWriteRepository;
        _productBasketReadRepository = productBasketReadRepository;
        _productBasketWriteRepository = productBasketWriteRepository;
        _productReadRepository = productReadRepository;
    }

    public async Task<BaseResponse<string>> Handle(AddBasketCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return new("User not fount", HttpStatusCode.Unauthorized);

        var basket = await _basketReadRepository.GetByBasketUser(userId, cancellationToken);
        if(basket is null)
        {
            basket = new Domain.Entities.Basket
            {
                UserId= userId,
            };
            await _basketWriteRepository.AddAsync(basket);
            await _basketWriteRepository.SaveChangeAsync();
        }

        var existingProduct=await _productBasketReadRepository.ExistProductInBasket(basket.Id,request.ProductId,cancellationToken);
        if (existingProduct is not null)
        {
            existingProduct.Quantity ++;
            existingProduct.TotalPrice = existingProduct.Quantity * existingProduct.Product.PriceAzn;
            _productBasketWriteRepository.Update(existingProduct);
        }

        else
        {
            var product = await _productReadRepository.GetByIdAsync(request.ProductId, cancellationToken);
            if (product is null)
                return new("Product not found", HttpStatusCode.NotFound);

            var productBasket = new ProductBasket
            {
                Id = Guid.NewGuid(),
                BasketId = basket.Id,
                ProductId = request.ProductId,
                Quantity = 1,
                TotalPrice = product.PriceAzn,
                PreviewImageUrl = product.PreviewImageUrl,
            };
            await _productBasketWriteRepository.AddAsync(productBasket);
        }
        await _productBasketWriteRepository.SaveChangeAsync();
        
        return new("Product successfully added to basket", true,HttpStatusCode.OK);
    }
}
