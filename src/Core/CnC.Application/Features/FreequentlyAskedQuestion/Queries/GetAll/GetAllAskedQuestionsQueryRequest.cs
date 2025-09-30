using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.FreequentlyAskedQuestion.Queries.GetAll;

public class GetAllAskedQuestionsQueryRequest:IRequest<BaseResponse<List<AskedQuestionResponse>>>
{
}
