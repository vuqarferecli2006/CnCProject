using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.Bio.Queries.GetAll;

public class GetAllBioQueryRequest:IRequest<BaseResponse<List<BioResponse>>>
{
}
