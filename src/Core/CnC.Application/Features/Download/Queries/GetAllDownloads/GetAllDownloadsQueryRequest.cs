using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.Download.Queries.GetAllDownloads;

public class GetAllDownloadsQueryRequest : IRequest<BaseResponse<List<DownloadResponse>>>
{
}
