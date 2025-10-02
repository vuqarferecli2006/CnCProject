using CnC.Application.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CnC.Application.Features.User.Commands.Profil;

public class ProfilImageUpdateRequest:IRequest<BaseResponse<string>>
{
    public IFormFile ProfilImage { get; set; } = null!;  
}
