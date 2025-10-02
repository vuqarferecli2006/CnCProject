using CnC.Application.Abstracts.Repositories.IInformationModelRepositories;
using CnC.Application.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace CnC.Application.Features.InformationModel.Commands.Update;

public class UpdateInformationModelHandler : IRequestHandler<UpdateInformationModelRequest, BaseResponse<string>>
{
    private readonly IInformationModelWriteRepository _informationModelWriteRepository;
    private readonly IInformationModelReadRepository _informationModelReadRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateInformationModelHandler(
        IInformationModelWriteRepository informationModelWriteRepository,
        IInformationModelReadRepository informationModelReadRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _informationModelWriteRepository = informationModelWriteRepository;
        _informationModelReadRepository = informationModelReadRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<BaseResponse<string>> Handle(UpdateInformationModelRequest request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return new("Unauthorized", HttpStatusCode.Unauthorized);

        var informationModel = await _informationModelReadRepository.GetByIdAsync(request.InformationModelId);
        if (informationModel == null)
            return new("Information model not found", HttpStatusCode.NotFound);

        informationModel.Description = request.Description;
        informationModel.VideoUrl = request.VideoUrl;

        _informationModelWriteRepository.Update(informationModel);
        await _informationModelWriteRepository.SaveChangeAsync();

        return new("Information model updated successfully", informationModel.Id.ToString(), true, HttpStatusCode.OK);
    }
}
