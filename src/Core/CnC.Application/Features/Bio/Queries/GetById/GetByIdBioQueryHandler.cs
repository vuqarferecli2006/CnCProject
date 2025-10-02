using CnC.Application.Abstracts.Repositories.IBioRepositories;
using CnC.Application.Shared.Responses;
using MediatR;
using System.Net;

namespace CnC.Application.Features.Bio.Queries.GetById;

public class GetByIdBioQueryHandler : IRequestHandler<GetByIdBioQueryRequest, BaseResponse<BioResponse>>
{
    public readonly IBioReadRepository _bioReadRepository;

    public GetByIdBioQueryHandler(IBioReadRepository bioReadRepository)
    {
        _bioReadRepository = bioReadRepository;
    }

    public async Task<BaseResponse<BioResponse>> Handle(GetByIdBioQueryRequest request, CancellationToken cancellationToken)
    {
        var bio = await _bioReadRepository.GetByIdAsync(request.BioId);
        if (bio is null)
            return new("Bio not found.", HttpStatusCode.NotFound);

        var response = new BioResponse
        {
            Id = bio.Id,
            Key = bio.Key,
            Value = bio.Value,
        };

        return new("Success",response,true,HttpStatusCode.OK);
    }
}
