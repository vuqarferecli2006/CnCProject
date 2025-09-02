using CnC.Application.Features.Product.Commands.Create;
using CnC.Application.Features.Product.Commands.Delete;
using CnC.Application.Features.Product.Commands.Update;
using CnC.Application.Features.ProductDescription.Commands.Create;
using CnC.Application.Features.ProductDescription.Commands.Update;
using CnC.Application.Features.ProductDescription.Queries.GetByIdDescription;
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

        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromForm] UpdateProductCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProductDescription([FromForm] UpdateProductDescriptionRequest request)
        {
            var response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct([FromQuery] DeleteProductCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductDescriptionById([FromQuery]GetByIdDescriptionQueryRequest request)
        {
            var response = await _mediator.Send(new GetByIdDescriptionQueryRequest { ProductId=request.ProductId,Currency=request.Currency} );
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
