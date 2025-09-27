using CnC.Application.Abstracts.Repositories.IInformationModelRepositories;
using CnC.Application.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace CnC.Application.Features.InformationModel.Create;

public class CreateInfromationModelHandler : IRequestHandler<CreateInfromationModelRequest, BaseResponse<string>>
{
    private readonly IInformationModelWriteRepository _informationModelWriteRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateInfromationModelHandler(IInformationModelWriteRepository informationModelWriteRepository,IHttpContextAccessor httpContextAccessor)
    {
        _informationModelWriteRepository = informationModelWriteRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<BaseResponse<string>> Handle(CreateInfromationModelRequest request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return new("Unauthorized", HttpStatusCode.Unauthorized);

        var informationModel = new Domain.Entities.InformationModel
        {
            VideoUrl = request.VideoUrl,
            Description = request.Description
        };

        await _informationModelWriteRepository.AddAsync(informationModel);
        await _informationModelWriteRepository.SaveChangeAsync();


        return new("Success", informationModel.ToString(), true, HttpStatusCode.Created);
    }
}
