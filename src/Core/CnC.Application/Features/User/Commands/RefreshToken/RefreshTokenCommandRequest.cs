using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.User.Commands.RefreshToken;

public class RefreshTokenCommandRequest:IRequest<BaseResponse<TokenResponse>>
{
    public string RefreshToken { get; init; } = string.Empty;

    public string AccessToken { get; init; } = string.Empty;
}
