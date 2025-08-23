using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Web;

namespace CnC.Application.Features.User.Commands.Email.ConfirmEmail;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommandRequest, BaseResponse<string>>
{
    private readonly UserManager<AppUser> _userManager;

    public ConfirmEmailCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<BaseResponse<string>> Handle(ConfirmEmailCommandRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user is null)
            return new("Email confirmation failed",HttpStatusCode.BadRequest);

        var decodeToken= HttpUtility.UrlDecode(request.Token);
        var result = await _userManager.ConfirmEmailAsync(user, request.Token);
        
        if (!result.Succeeded)
        {
            var resultDecode = await _userManager.ConfirmEmailAsync(user, decodeToken);
 
            if(!resultDecode.Succeeded)
                return new("Email confirmation failed",HttpStatusCode.BadRequest);
        }
    
        return new("Email confirmed successfully", true, HttpStatusCode.OK);
    }
}
