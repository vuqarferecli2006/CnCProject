using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.User.Commands.Profil.FullName;

public class FullNameUpdateCommandRequest:IRequest<BaseResponse<string>>
{
    public string FullName { get; set; } = null!;
}
