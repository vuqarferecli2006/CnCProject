using CnC.Application.Features.Basket.Commands.Create;
using CnC.Application.Features.Basket.Commands.Delete;
using CnC.Application.Features.Basket.Queries.GetBasket;
using CnC.Application.Shared.Permissions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CnC.WepApi.Controllers.Basket
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BasketsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Policy =Permission.Basket.AddBasket)]
        public async Task<IActionResult> AddProductToBasket([FromBody] AddBasketCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }
         
        [HttpDelete]
        [Authorize(Policy =Permission.Basket.DeleteBasket)]
        public async Task<IActionResult> DeleteProductInBasket([FromBody] DeleteProductInBasketCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet]
        [Authorize(Policy =Permission.Basket.GetAllBasket)]
        public async Task<IActionResult> GetProductInBasket([FromQuery] GetBasketQueriesRequest request)
        {
            var response= await _mediator.Send(new GetBasketQueriesRequest { Currency =request.Currency});
            return StatusCode((int)response.StatusCode,response);
        }
    }
}
