using CnC.Application.Shared.Responses;
using CnC.Domain.Enums;
using MediatR;

namespace CnC.Application.Features.UserCreationByAdmin.Commands.RegisterUserByAdmin;

public class CreateUserCommandRequest:IRequest<BaseResponse<string>>
{
    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public PlatformRole Role { get; set; }
}
