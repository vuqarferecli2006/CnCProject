using CnC.Application.Abstracts.Repositories.IDownloadRepositories;
using CnC.Application.Abstracts.Repositories.IOrderRepositories;
using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace CnC.Application.Features.Download.Queries.GetDownloadsByProduct;

public class GetDownloadByProductIdHandler : IRequestHandler<GetDownloadByProductIdRequest, BaseResponse<List<string>>>
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IOrderReadRepository _orderReadRepository;
    private readonly IDownloadReadRepository _downloadReadRepository;

    public GetDownloadByProductIdHandler(
        IHttpContextAccessor contextAccessor,
        IOrderReadRepository orderReadRepository,
        IDownloadReadRepository downloadReadRepository)
    {
        _contextAccessor = contextAccessor;
        _orderReadRepository = orderReadRepository;
        _downloadReadRepository = downloadReadRepository;
    }

    public async Task<BaseResponse<List<string>>> Handle(GetDownloadByProductIdRequest request, CancellationToken cancellationToken)
    {
        var userId = _contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return new("User not found", HttpStatusCode.Unauthorized);

        var paidOrders = await _orderReadRepository.GetPaidOrdersWithProductsAsync(userId, cancellationToken);

        if (!paidOrders.Any())
            return new("No paid orders found", HttpStatusCode.NotFound);

        var orderProducts = paidOrders
            .SelectMany(o => o.OrderProducts)
            .Where(op => op.ProductId == request.ProductId)
            .ToList();

        if (!orderProducts.Any())
            return new("Product not found in paid orders", HttpStatusCode.NotFound);

        var fileUrls = new List<string>();
        foreach (var op in orderProducts)
        {
            var downloads = await _downloadReadRepository.GetByOrderProductIdAsync(op.Id, cancellationToken);

            fileUrls.AddRange(downloads.Select(d => d.FileUrl));
        }

        if (!fileUrls.Any())
            return new("No downloaded files for this product", HttpStatusCode.NotFound);

        return new("Success", fileUrls.Distinct().ToList(), true, HttpStatusCode.OK);
    }
}
