using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.InformationModel.Commands.Update;

public class UpdateInformationModelRequest : IRequest<BaseResponse<string>>
{
    public Guid InformationModelId { get; set; }
    public string Description { get; set; } = null!;
    public string VideoUrl { get; set; } = null!;
}
