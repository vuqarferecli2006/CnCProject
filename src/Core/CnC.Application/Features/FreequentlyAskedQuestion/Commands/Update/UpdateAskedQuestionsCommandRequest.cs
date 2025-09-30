using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.FreequentlyAskedQuestion.Commands.Update;

public class UpdateAskedQuestionsCommandRequest : IRequest<BaseResponse<string>>
{
    public Guid AskedQuestionId { get; set; }
    public string VideoUrl { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Questions { get; set; } = null!;
    public string Answer { get; set; } = null!;
}
