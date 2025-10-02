using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.Bio.Commands.Update;

public class UpdateBioCommandRequest : IRequest<BaseResponse<string>>
{
    public Guid BioId { get; set; }
    public string Key { get; set; } = null!;
    public string Value { get; set; } = null!;
}
