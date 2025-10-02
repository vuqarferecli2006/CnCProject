using CnC.Application.Abstracts.Repositories.IAskedQuestionsRepositories;
using CnC.Application.Shared.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CnC.Application.Features.FreequentlyAskedQuestion.Queries.GetAll;

public class GetAllAskedQuestionsHandler : IRequestHandler<GetAllAskedQuestionsQueryRequest, BaseResponse<List<AskedQuestionResponse>>>
{
    private readonly IReadAskedQuestionsRepository _readAskedQuestionsRepository;

    public GetAllAskedQuestionsHandler(IReadAskedQuestionsRepository readAskedQuestionsRepository)
    {
        _readAskedQuestionsRepository = readAskedQuestionsRepository;
    }

    public async Task<BaseResponse<List<AskedQuestionResponse>>> Handle(GetAllAskedQuestionsQueryRequest request, CancellationToken cancellationToken)
    {
        var questions = await _readAskedQuestionsRepository.GetAll().ToListAsync();

        var result = questions.Select(q => new AskedQuestionResponse
        {
            Id = q.Id,
            VideoUrl = q.VideoUrl,
            Description = q.Description,
            Question = q.Question,
            Answer = q.Answer
        }).ToList();

        return new("Success", result, true, HttpStatusCode.OK);
    }
}

