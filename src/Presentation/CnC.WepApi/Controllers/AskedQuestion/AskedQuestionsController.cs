using CnC.Application.Features.FreequentlyAskedQuestion.Commands.Create;
using CnC.Application.Features.FreequentlyAskedQuestion.Commands.Update;
using CnC.Application.Features.FreequentlyAskedQuestion.Queries.GetAll;
using CnC.Application.Shared.Permissions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CnC.WepApi.Controllers.AskedQuestion
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AskedQuestionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AskedQuestionsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [Authorize(Policy =Permission.AskedQuestions.AskedQuestionsCreate)]
        public async Task<IActionResult> CreateAskedQuestions([FromBody] CreateAskedQuestionsCommandRequest request)
        {
            var response=await _mediator.Send(request);
            return StatusCode((int)response.StatusCode,response);
        }

        [HttpPut]
        [Authorize(Policy =Permission.AskedQuestions.AskedQuestionsUpdate)]
        public async Task<IActionResult> UpdateAskedQuestions([FromBody] UpdateAskedQuestionsCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAskedQuestions()
        {
            var response = await _mediator.Send(new GetAllAskedQuestionsQueryRequest());
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
