using CnC.Application.Abstracts.Repositories.ICurrencyRateRepository;
using CnC.Application.Abstracts.Services;
using CnC.Application.DTOs.ElasticSearch;
using CnC.Application.Features.Product.Commands.Create;
using CnC.Application.Features.Product.Commands.Delete;
using CnC.Application.Features.Product.Commands.Update;
using CnC.Application.Features.ProductDescription.Commands.Create;
using CnC.Application.Features.ProductDescription.Commands.Update;
using CnC.Application.Features.ProductDescription.Queries.GetByIdDescription;
using CnC.Application.Features.ProductView.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CnC.WepApi.Controllers.Product
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IElasticProductService _elasticProductService;
        public ProductsController(IMediator mediator, IElasticProductService elasticProductService)
        {
            _mediator = mediator;
            _elasticProductService = elasticProductService;
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
        public async Task<IActionResult> GetBySlugProductDescription([FromQuery] GetBySlugDescriptionQueryRequest request)
        {
            var response = await _mediator.Send(new GetBySlugDescriptionQueryRequest { Slug = request.Slug, Currency = request.Currency });
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] ElasticSearchProductDto dto, CancellationToken cancellationToken)
        {
            var result = await _elasticProductService.SearchAsync(dto, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> FilterProduct([FromQuery] FilterProductDto dto, CancellationToken cancellationToken)
        {
            var result = await _elasticProductService.FilterProductsAsync(dto, cancellationToken);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetProductView([FromQuery] GetUserProductViewsQueryRequest request)
        {
            var result = await _mediator.Send(new GetUserProductViewsQueryRequest { Currency = request.Currency });
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
