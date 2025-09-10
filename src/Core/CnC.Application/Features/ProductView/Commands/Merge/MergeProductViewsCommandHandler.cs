using CnC.Application.Abstracts.Repositories.IProductViewRepositories;
using CnC.Application.Shared.Responses;
using MediatR;
using System.Net;

namespace CnC.Application.Features.ProductView.Commands.Merge;

public class MergeProductViewsCommandHandler: IRequestHandler<MergeProductViewsCommandRequest, BaseResponse<string>>
{
    private readonly IProductViewReadRepository _readRepo;
    private readonly IProductViewWriteRepository _writeRepo;

    public MergeProductViewsCommandHandler(IProductViewReadRepository readRepo,IProductViewWriteRepository writeRepo)
    {
        _readRepo = readRepo;
        _writeRepo = writeRepo;
    }

    public async Task<BaseResponse<string>> Handle(MergeProductViewsCommandRequest request,CancellationToken cancellationToken)
    {
        var sessionViews = await _readRepo.GetBySessionIdAsync(request.SessionId, cancellationToken);
        if (!sessionViews.Any())
            return new("No session views to merge", HttpStatusCode.NotFound);

        var userViews = await _readRepo.GetByUserIdAsync(request.UserId, cancellationToken);

        var newViews = sessionViews
            .Where(sv => !userViews.Any(uv => uv.ProductId == sv.ProductId))
            .Select(v => new Domain.Entities.ProductView
            {
                ProductId = v.ProductId,
                UserId = request.UserId,
                ViewedAt = v.ViewedAt
            }).ToList();

        await _writeRepo.BulkAddAsync(newViews, cancellationToken);
        await _writeRepo.BulkDeleteAsync(sessionViews, cancellationToken);
        await _writeRepo.SaveChangeAsync();

        return new("Merged successfully", "OK", true, HttpStatusCode.OK);
    }
}


