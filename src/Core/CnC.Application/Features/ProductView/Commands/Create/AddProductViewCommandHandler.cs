using CnC.Application.Abstracts.Repositories.IProductViewRepositories;
using CnC.Application.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace CnC.Application.Features.ProductView.Commands.Create;

public class AddProductViewCommandHandler : IRequestHandler<AddProductViewCommandRequest, BaseResponse<string>>
{
    private readonly IProductViewWriteRepository _productViewWriteRepository;
    private readonly IProductViewReadRepository _productViewReadRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AddProductViewCommandHandler(
        IProductViewWriteRepository productViewWriteRepository,
        IProductViewReadRepository productViewReadRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _productViewWriteRepository = productViewWriteRepository;
        _productViewReadRepository = productViewReadRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<BaseResponse<string>> Handle(AddProductViewCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return new("Unauthorized", HttpStatusCode.Unauthorized);

        var existingView = await _productViewReadRepository
            .GetSingleAsync(userId, request.ProductId, cancellationToken);

        if (existingView != null)
        {
            existingView.UpdatedAt = DateTime.UtcNow;
            _productViewWriteRepository.Update(existingView);
        }
        else
        {
            var userViews = await _productViewReadRepository
                .GetAll().Where(pv => pv.UserId == userId)
                .OrderBy(pv => pv.UpdatedAt)
                .ToListAsync(cancellationToken);

            if (userViews.Count >= 12)
            {
                var oldestView = userViews.First();
                _productViewWriteRepository.Delete(oldestView);
            }

            var newView = new Domain.Entities.ProductView
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ProductId = request.ProductId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _productViewWriteRepository.AddAsync(newView);
        }

        await _productViewWriteRepository.SaveChangeAsync();

        return new("ProductView successfully added/updated", "Success", true, HttpStatusCode.OK);
    }
}

