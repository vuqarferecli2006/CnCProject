using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.FreequentlyAskedQuestion.Commands.Create;

public class CreateAskedQuestionsCommandRequest:IRequest<BaseResponse<string>>
{
    public string VideoUrl { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Questions {  get; set; } = null!;

    public string Answer { get; set; } = null!;

}
