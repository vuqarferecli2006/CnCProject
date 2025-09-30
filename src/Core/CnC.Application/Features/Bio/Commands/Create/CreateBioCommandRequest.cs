using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.Bio.Commands.Create;

public class CreateBioCommandRequest:IRequest<BaseResponse<string>>
{
    public string Key { get; set; } = null!;
    public string Value { get; set; } = null!;
}
