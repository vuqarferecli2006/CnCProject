using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.User.Queries.ResetConfirmEmail;

public class ResetConfirmEmailQueryRequest:IRequest<ResetPasswordModelResponse>
{
    public string Email { get; set; } = null!;
    public string Token { get; set; } = null!;
}
