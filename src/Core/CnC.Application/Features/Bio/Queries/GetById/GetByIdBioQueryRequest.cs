using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.Bio.Queries.GetById;

public class GetByIdBioQueryRequest:IRequest<BaseResponse<BioResponse>>
{
    public Guid BioId { get; set; }
}
