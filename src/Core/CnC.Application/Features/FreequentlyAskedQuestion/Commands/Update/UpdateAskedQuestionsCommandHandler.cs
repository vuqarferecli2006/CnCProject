using CnC.Application.Abstracts.Repositories.IAskedQuestionsRepositories;
using CnC.Application.Shared.Responses;
using MediatR;
using System.Net;

namespace CnC.Application.Features.FreequentlyAskedQuestion.Commands.Update;

public class UpdateAskedQuestionsCommandHandler : IRequestHandler<UpdateAskedQuestionsCommandRequest, BaseResponse<string>>
{
    private readonly IReadAskedQuestionsRepository _readAskedQuestionsRepository;
    private readonly IWriteAskedQuestionsRepository _writeAskedQuestionsRepository;

    public UpdateAskedQuestionsCommandHandler(
        IReadAskedQuestionsRepository readAskedQuestionsRepository,
        IWriteAskedQuestionsRepository writeAskedQuestionsRepository)
    {
        _readAskedQuestionsRepository = readAskedQuestionsRepository;
        _writeAskedQuestionsRepository = writeAskedQuestionsRepository;
    }

    public async Task<BaseResponse<string>> Handle(UpdateAskedQuestionsCommandRequest request, CancellationToken cancellationToken)
    {
        var askedQuestion = await _readAskedQuestionsRepository.GetByIdAsync(request.AskedQuestionId);
        if (askedQuestion == null)
            return new("Asked question not found", HttpStatusCode.NotFound);

        askedQuestion.VideoUrl = request.VideoUrl;
        askedQuestion.Description = request.Description;
        askedQuestion.Question = request.Questions;
        askedQuestion.Answer = request.Answer;

        _writeAskedQuestionsRepository.Update(askedQuestion);
        await _writeAskedQuestionsRepository.SaveChangeAsync();

        return new("Asked question updated successfully", askedQuestion.Id.ToString(), true, HttpStatusCode.OK);
    }
}