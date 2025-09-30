using CnC.Application.Abstracts.Repositories.IAskedQuestionsRepositories;
using CnC.Application.Shared.Responses;
using MediatR;
using System.Net;

namespace CnC.Application.Features.FreequentlyAskedQuestion.Commands.Create;

public class CreateAskedQuestionsCommandHandler : IRequestHandler<CreateAskedQuestionsCommandRequest, BaseResponse<string>>
{
    private readonly IWriteAskedQuestionsRepository _writeAskedQuestionsRepository;

    public CreateAskedQuestionsCommandHandler(IWriteAskedQuestionsRepository writeAskedQuestionsRepository)
    {
        _writeAskedQuestionsRepository = writeAskedQuestionsRepository;
    }

    public async Task<BaseResponse<string>> Handle(CreateAskedQuestionsCommandRequest request, CancellationToken cancellationToken)
    {
        var askedQuestion = new Domain.Entities.FreequentlyAskedQuestion
        {
            VideoUrl = request.VideoUrl,
            Description = request.Description,
            Answer= request.Answer,
            Question=request.Questions
        };

        await _writeAskedQuestionsRepository.AddAsync(askedQuestion);
        await _writeAskedQuestionsRepository.SaveChangeAsync();

        return new("Ask and Questions successfully created",HttpStatusCode.Created);
    }
}
