using CnC.Application.Features.InformationModel.Create;
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
    }
}
