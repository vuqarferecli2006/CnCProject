using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Text;
using System.Web;

namespace CnC.Application.Features.User.Queries.ResetConfirmEmail;

public class ResetConfirmEmailQueryHandler : IRequestHandler<ResetConfirmEmailQueryRequest, ResetPasswordModelResponse>
{
    private readonly UserManager<AppUser> _userManager;

    public ResetConfirmEmailQueryHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ResetPasswordModelResponse> Handle(ResetConfirmEmailQueryRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return new("User not found", HttpStatusCode.NotFound);

        return new(request.Email, request.Token, "Token is valid", true, HttpStatusCode.OK);
    }
}


