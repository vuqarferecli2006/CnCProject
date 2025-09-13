using CnC.Application.Abstracts.Repositories.IBasketRepositories;
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

    public DeleteProductInBasketCommandHandler(IHttpContextAccessor contextAccessor, 
                                            IBasketReadRepository basketReadRepository, 
                                            IBasketWriteRepository basketWriteRepository, 
                                            IProductBasketReadRepository productBasketReadRepository, 
                                            IProductBasketWriteRepository productBasketWriteRepository)
    {
        _contextAccessor = contextAccessor;
        _basketReadRepository = basketReadRepository;
        _productBasketReadRepository = productBasketReadRepository;
        _productBasketWriteRepository = productBasketWriteRepository;
    }

    public async Task<BaseResponse<string>> Handle(DeleteProductInBasketCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return new("User not found", HttpStatusCode.Unauthorized);

        var basket = await _basketReadRepository.GetByBasketUser(userId,cancellationToken);
        if(basket is null)
            return new("Basket not found",HttpStatusCode.NotFound);

        var existProduct = await _productBasketReadRepository.ExistProductInBasket(basket.Id, request.ProductId, cancellationToken);
        if(existProduct is null)
            return new("Product not found in basket",HttpStatusCode.NotFound);

        _productBasketWriteRepository.Delete(existProduct);
        await _productBasketWriteRepository.SaveChangeAsync();

        return new("Removed successfully", true, HttpStatusCode.OK);
    }
}
