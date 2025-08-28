using CnC.Application.Features.Product.Commands.Create;
using CnC.Application.Features.ProductDescription.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CnC.WepApi.Controllers.Product
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductDescription([FromForm] CreateProductDescriptionRequest request)
        {
            var response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
