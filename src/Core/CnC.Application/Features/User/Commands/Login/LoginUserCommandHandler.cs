using CnC.Application.Abstracts.Services;
using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace CnC.Application.Features.User.Commands.Login;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, BaseResponse<TokenResponse>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IJwtService _jwtService;   


    public LoginUserCommandHandler(UserManager<AppUser> userManager, 
                                 SignInManager<AppUser> signInManager,
                                  IJwtService jwtService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
    }

    public async Task<BaseResponse<TokenResponse>> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
    {
        var existingUser =await _userManager.FindByEmailAsync(request.Email);
        if (existingUser is null)
            return new("Email or password incorrect",HttpStatusCode.NotFound);

        if(!existingUser.EmailConfirmed)
            return new("Please confirm your email",HttpStatusCode.BadRequest);

        SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(existingUser, request.Password, true);

        if (!signInResult.Succeeded)
            return new("Email or password incorrect", HttpStatusCode.NotFound);


        var token =await _jwtService.GenerateJwttoken(existingUser);

        return new("Login Seccussful",token,true,HttpStatusCode.OK);
    }
}
