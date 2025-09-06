using CnC.Application.Abstracts.Repositories.IFavouriteRepositories;
using CnC.Application.Abstracts.Repositories.IProductRepositories;
using CnC.Application.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace CnC.Application.Features.Favourite.Commands.Delete;

public class DeleteFavouriteCommandHandler : IRequestHandler<DeleteFavouriteCommandRequest, BaseResponse<string>>
{
    private readonly IFavouriteReadRepository _favouriteReadRepository;
    private readonly IFavouriteWriteRepository _favouriteWriteRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProductReadRepository _productReadRepository;

    public DeleteFavouriteCommandHandler(IFavouriteReadRepository favouriteReadRepository,
                                    IFavouriteWriteRepository favouriteWriteRepository,
                                    IHttpContextAccessor httpContextAccessor, 
                                    IProductReadRepository productReadRepository)
    {
        _favouriteReadRepository = favouriteReadRepository;
        _favouriteWriteRepository = favouriteWriteRepository;
        _httpContextAccessor = httpContextAccessor;
        _productReadRepository = productReadRepository;
    }

    public async Task<BaseResponse<string>> Handle(DeleteFavouriteCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return new("User not found", HttpStatusCode.Unauthorized);

        var favouriteExist = await _favouriteReadRepository.GetFavouriteAsync(request.ProductId, userId);
        if (favouriteExist is null)
            return new("Product not found in favourite", HttpStatusCode.NotFound);
        
        _favouriteWriteRepository.Delete(favouriteExist);
        await _favouriteWriteRepository.SaveChangeAsync();

        return new("Product successfully removed from favorites", HttpStatusCode.OK);
    }
}
