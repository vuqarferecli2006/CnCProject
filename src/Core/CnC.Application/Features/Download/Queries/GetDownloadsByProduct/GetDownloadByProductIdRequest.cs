using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.Download.Queries.GetDownloadsByProduct;

public class GetDownloadByProductIdRequest:IRequest<BaseResponse<List<string>>>
{
    public Guid ProductId { get; set; }
}
