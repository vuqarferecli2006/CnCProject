using CnC.Application.Features.Bio.Commands.Create;
using CnC.Application.Features.Bio.Commands.Update;
using CnC.Application.Features.Bio.Queries.GetAll;
using CnC.Application.Features.Bio.Queries.GetById;
using CnC.Application.Shared.Permissions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace CnC.WepApi.Controllers.Bio
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BiosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BiosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Policy =Permission.Bio.CreateBio)]
        public async Task<IActionResult> CreateBio([FromBody]CreateBioCommandRequest request)
        {
            var response=await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut]
        [Authorize(Policy =Permission.Bio.UpdateBio)]
        public async Task<IActionResult> UpdateBio([FromBody] UpdateBioCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet]
        [Authorize(Policy =Permission.Bio.GetAll)]
        public async Task<IActionResult> GetAllBio()
        {
            var response = await _mediator.Send(new GetAllBioQueryRequest());
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByIdBio([FromQuery] GetByIdBioQueryRequest request)
        {
            var response = await _mediator.Send(new GetByIdBioQueryRequest { BioId = request.BioId });
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
