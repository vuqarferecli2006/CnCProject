using CnC.Application.Abstracts.Repositories.IBioRepositories;
using CnC.Application.Shared.Responses;
using MediatR;
using System.Net;

namespace CnC.Application.Features.Bio.Commands.Update;

public class UpdateBioCommandHandler : IRequestHandler<UpdateBioCommandRequest, BaseResponse<string>>
{
    private readonly IBioWriteRepository _bioWriteRepository;
    private readonly IBioReadRepository _bioReadRepository;

    public UpdateBioCommandHandler(IBioWriteRepository bioWriteRepository, IBioReadRepository bioReadRepository)
    {
        _bioWriteRepository = bioWriteRepository;
        _bioReadRepository = bioReadRepository;
    }

    public async Task<BaseResponse<string>> Handle(UpdateBioCommandRequest request, CancellationToken cancellationToken)
    {
        var bio = await _bioReadRepository.GetByIdAsync(request.BioId);
        if (bio == null)
            return new("Bio not found", HttpStatusCode.NotFound);

        bio.Key = request.Key;
        bio.Value = request.Value;

        _bioWriteRepository.Update(bio);
        await _bioWriteRepository.SaveChangeAsync();

        return new("Bio updated successfully", bio.Id.ToString(), true, HttpStatusCode.OK);
    }
}