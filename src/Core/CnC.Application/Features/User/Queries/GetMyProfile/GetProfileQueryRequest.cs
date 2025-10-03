using CnC.Application.Shared.Responses;
using CnC.Application.Shared.Responsesl;
using MediatR;

namespace CnC.Application.Features.User.Queries.GetMyProfile;

public class GetProfileQueryRequest:IRequest<BaseResponse<UserProfileResponse>>
{
}
