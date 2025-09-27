using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.InformationModel.Create;

public class CreateInfromationModelRequest:IRequest<BaseResponse<string>>
{
    public string Description { get; set; } = null!;

    public string VideoUrl { get; set; } = null!;
}
