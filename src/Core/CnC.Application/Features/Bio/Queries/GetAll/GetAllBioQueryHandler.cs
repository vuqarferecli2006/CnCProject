using CnC.Application.Abstracts.Repositories.IBioRepositories;
using CnC.Application.Shared.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CnC.Application.Features.Bio.Queries.GetAll;

public class GetAllBioQueryHandler : IRequestHandler<GetAllBioQueryRequest, BaseResponse<List<BioResponse>>>
{
    private readonly IBioReadRepository _bioReadRepository;

    public GetAllBioQueryHandler(IBioReadRepository bioReadRepository)
    {
        _bioReadRepository = bioReadRepository;
    }

    public async Task<BaseResponse<List<BioResponse>>> Handle(GetAllBioQueryRequest request, CancellationToken cancellationToken)
    {
        var bios=await _bioReadRepository.GetAll().ToListAsync();

        var result = bios.Select(b => new BioResponse
        {
            Id = b.Id,
            Key = b.Key,
            Value = b.Value
        }).ToList();

        return new("Success", result, true, HttpStatusCode.OK);
    }
}
