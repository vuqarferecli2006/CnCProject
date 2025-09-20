using CnC.Application.Abstracts.Repositories.IOrderRepositories;
using CnC.Application.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace CnC.Application.Features.Download.Queries.GetAllDownloads;

public class GetAllDownloadsQueryHandler : IRequestHandler<GetAllDownloadsQueryRequest, BaseResponse<List<DownloadResponse>>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IOrderReadRepository _orderReadRepository;

    public GetAllDownloadsQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        IOrderReadRepository orderReadRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _orderReadRepository = orderReadRepository;
    }

    public async Task<BaseResponse<List<DownloadResponse>>> Handle(GetAllDownloadsQueryRequest request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return new("User not found", HttpStatusCode.Unauthorized);

        var paidOrders = await _orderReadRepository.GetPaidOrdersWithProductsAsync(userId, cancellationToken);

        if (!paidOrders.Any())
            return new("No paid orders found", HttpStatusCode.NotFound);

        var response = paidOrders.Select(order => new DownloadResponse
        {
            OrderId = order.Id,
            IsPaid = order.isPaid,
            OrderDate = order.OrderDate,
            Products = order.OrderProducts.Select(op => new DownloadProductResponse
            {
                ProductId = op.ProductId,
                ProductName = op.Product.Name,
                FileUrl = op.Product.ProductDescription?.ProductFiles?
                                .Select(f => f.FileUrl)
                                .ToList() ?? new List<string>()
            }).ToList()
        }).ToList();

        return new("Success", response, true, HttpStatusCode.OK);
    }
}
