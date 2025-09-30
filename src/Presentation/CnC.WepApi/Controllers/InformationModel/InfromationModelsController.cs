using CnC.Application.Features.InformationModel.Commands.Create;
using CnC.Application.Features.InformationModel.Commands.Update;
using CnC.Application.Features.InformationModel.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace CnC.WepApi.Controllers.InformationModel
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfromationModelsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InfromationModelsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> CreateInformationResult([FromBody] CreateInfromationModelRequest request)
        {
            var response= await _mediator.Send(request);
            return StatusCode((int)response.StatusCode,response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateInformationModel([FromBody]UpdateInformationModelRequest request)
        {
            var response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInformationModel()
        {
            var response=await _mediator.Send(new GetAllInformationModelsRequestQuery());
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
