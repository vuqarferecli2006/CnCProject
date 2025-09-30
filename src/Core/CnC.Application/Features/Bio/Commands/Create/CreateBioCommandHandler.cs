using CnC.Application.Abstracts.Repositories.IBioRepositories;
using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using MediatR;
using System.Net;

namespace CnC.Application.Features.Bio.Commands.Create;

public class CreateBioCommandHandler : IRequestHandler<CreateBioCommandRequest, BaseResponse<string>>
{
    private readonly IBioWriteRepository _bioWriteRepository;

    public CreateBioCommandHandler(IBioWriteRepository bioWriteRepository)
    {
        _bioWriteRepository = bioWriteRepository;
    }

    public async Task<BaseResponse<string>> Handle(CreateBioCommandRequest request, CancellationToken cancellationToken)
    {
        var bio = new Domain.Entities.Bio
        {
            Key = request.Key,
            Value = request.Value,
        };

        await _bioWriteRepository.AddAsync(bio);
        await _bioWriteRepository.SaveChangeAsync();

        return new("Bio created successfully",HttpStatusCode.Created);
    }
}
