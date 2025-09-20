using CnC.Application.Abstracts.Repositories.IFavouriteRepositories;
using CnC.Application.Abstracts.Repositories.IProductRepositories;
using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace CnC.Application.Features.Favourite.Commands.Create;

public class CreateFavouriteCommandHandler : IRequestHandler<CreateFavouriteCommandRequest, BaseResponse<string>>
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IProductReadRepository _productReadRepository;
    private readonly IFavouriteReadRepository _favouriteReadRepository;
    private readonly IFavouriteWriteRepository _favouriteWriteRepository;

    public CreateFavouriteCommandHandler(IHttpContextAccessor contextAccessor, 
                                    IProductReadRepository productReadRepository, 
                                    IFavouriteReadRepository favouriteReadRepository,
                                    IFavouriteWriteRepository favouriteWriteRepository)
    {
        _contextAccessor = contextAccessor;
        _productReadRepository = productReadRepository;
        _favouriteReadRepository = favouriteReadRepository;
        _favouriteWriteRepository = favouriteWriteRepository;
    }

    public async Task<BaseResponse<string>> Handle(CreateFavouriteCommandRequest request, CancellationToken cancellationToken)
    {
        var userId= _contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if(string.IsNullOrEmpty(userId)) 
            return new("User not found",HttpStatusCode.Unauthorized);

        var product=await _productReadRepository.GetByIdAsync(request.ProductId);

        if(product is null)
            return new("Product not found", HttpStatusCode.NotFound);


        var productExist = await _favouriteReadRepository.IsProductFavouriteAsync(request.ProductId, userId);
        if (productExist)
            return new("Product already exist",HttpStatusCode.BadRequest);

        var favourite = new BookMark
        { 
            Id=Guid.NewGuid(),
            ProductId = request.ProductId,
            UserId = userId,
        };

        await _favouriteWriteRepository.AddAsync(favourite);
        await _favouriteWriteRepository.SaveChangeAsync();

        return new("Product successfully added to favorites", true,HttpStatusCode.OK);
    }
}
