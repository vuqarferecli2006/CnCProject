using CnC.Application.Features.Download.Queries.GetAllDownloads;
using CnC.Application.Features.Download.Queries.GetDownloadsByProduct;
using CnC.Application.Shared.Permissions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace CnC.WepApi.Controllers.Downoad
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DownloadsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DownloadsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        [Authorize(Policy =Permission.Download.GetAllDownload)]
        public async Task<IActionResult> GetAllDownloads()
        {
            var response=await _mediator.Send(new GetAllDownloadsQueryRequest());
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet]
        [Authorize(Policy =Permission.Download.GetByIdDownload)]
        public async Task<IActionResult> GetByDownload([FromQuery] GetDownloadByProductIdRequest request)
        {
            var response = await _mediator.Send(new GetDownloadByProductIdRequest { ProductId=request.ProductId});
            return StatusCode((int)response.StatusCode,response);
        }
    }
}
