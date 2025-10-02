using CnC.Application.Abstracts.Repositories.IInformationModelRepositories;
using CnC.Application.Shared.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CnC.Application.Features.InformationModel.Queries.GetAll;

public class GetAllInformationModelsHandler : IRequestHandler<GetAllInformationModelsRequestQuery, BaseResponse<List<InformationModelResponse>>>
{
    private readonly IInformationModelReadRepository _informationModelReadRepository;

    public GetAllInformationModelsHandler(IInformationModelReadRepository informationModelReadRepository)
    {
        _informationModelReadRepository = informationModelReadRepository;
    }

    public async Task<BaseResponse<List<InformationModelResponse>>> Handle(GetAllInformationModelsRequestQuery request, CancellationToken cancellationToken)
    {
        var informationModels =await _informationModelReadRepository.GetAll().ToListAsync();

        var result = informationModels.Select(im => new InformationModelResponse
        {
            Id = im.Id,
            Description = im.Description,
            VideoUrl = im.VideoUrl
        }).ToList();

        return new("Success", result, true, HttpStatusCode.OK);
    }
}

