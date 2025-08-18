using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using CnC.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Text;

namespace CnC.Application.Features.User.Commands.Register;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommandRequest, BaseResponse<string>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RegisterUserCommandHandler(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<BaseResponse<string>> Handle(RegisterUserCommandRequest request, CancellationToken cancellationToken)
    {
        var existingEmail = await _userManager.FindByEmailAsync(request.Email);

        if (existingEmail is not null)
            return new("This email is already registered", HttpStatusCode.BadRequest);

        if (request.RoleId != MarketPlaceRole.Buyer && request.RoleId != MarketPlaceRole.Seller)
            return new("Invalid role", HttpStatusCode.NotFound);

        var user = new AppUser
        {
            FullName = request.FullName,
            Email = request.Email,
            UserName = request.Email,
            PasswordHash = request.Password,
            ProfilePictureUrl = request.ProfilPictureUrl
        };

        IdentityResult identityResult = await _userManager.CreateAsync(user, request.Password);
        if (!identityResult.Succeeded)
        {
            StringBuilder errorMessage = new();
            foreach (var error in identityResult.Errors)
            {
                errorMessage.AppendLine(error.Description);
            }
            return new(errorMessage.ToString(), HttpStatusCode.BadRequest);
        }

        string roleName = request.RoleId switch
        {
            MarketPlaceRole.Buyer => "Buyer",
            MarketPlaceRole.Seller => "Seller",
            _ => throw new ArgumentOutOfRangeException(nameof(request.RoleId), "Invalid user role")
        };

        if (roleName is null)
            return new("Role not found", HttpStatusCode.NotFound);

        var roleResult=await _userManager.AddToRoleAsync(user, roleName);
        
        if (!roleResult.Succeeded)
            return new("Failed to assign role to user", HttpStatusCode.BadRequest);

        return new("User registered successfully", user.Id, true, HttpStatusCode.Created);
    }
}
