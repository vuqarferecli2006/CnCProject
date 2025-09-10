using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.ProductView.Commands.Merge;

public class MergeProductViewsCommandRequest:IRequest<BaseResponse<string>>
{
    public string SessionId { get; set; } = null!;
}
