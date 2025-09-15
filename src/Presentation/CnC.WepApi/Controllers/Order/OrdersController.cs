using CnC.Application.Features.Order.Commands.ChooseForOrder;
using CnC.Application.Features.Order.Commands.Create;
using CnC.Application.Features.Order.Commands.RemoveForOrder;
using CnC.Application.Features.Order.Queries.GetOrder;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CnC.WepApi.Controllers.Order
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrders([FromBody] CreateOrderCommandRequest request)
        {
            var response=await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut]
        public async Task<IActionResult> ChooseProductForOrder([FromBody] ChooseForOrderCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut]
        public async Task<IActionResult> CancelProductForOrder([FromBody] RemoveForOrderCommandRequest request)
        {
            var response=await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet] 
        public async Task<IActionResult> GetOrder([FromQuery] GetOrderQueryRequest request)
        {
            var response= await _mediator.Send(new GetOrderQueryRequest { Currency=request.Currency});
            return StatusCode((int)response.StatusCode,response);
        }
    }
}
