using CnC.Application.Features.Download.Queries.GetAllDownloads;
using CnC.Application.Features.Download.Queries.GetDownloadsByProduct;
using MediatR;
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
        public async Task<IActionResult> GetAllDownloads()
        {
            var response=await _mediator.Send(new GetAllDownloadsQueryRequest());
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByDownload([FromQuery] GetDownloadByProductIdRequest request)
        {
            var response = await _mediator.Send(new GetDownloadByProductIdRequest { ProductId=request.ProductId});
            return StatusCode((int)response.StatusCode,response);
        }
    }
}
