using CnC.Application.Features.Order.Commands.ChooseForOrder;
using CnC.Application.Features.Order.Commands.Create;
using CnC.Application.Features.Order.Commands.RemoveForOrder;
using CnC.Application.Features.Order.Queries.GetOrder;
using CnC.Application.Features.Order.Queries.GetPaidOrder;
using CnC.Application.Shared.Permissions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Policy = Permission.Order.CreateOrder)]
        public async Task<IActionResult> CreateOrders([FromBody] CreateOrderCommandRequest request)
        {
            var response=await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut]
        [Authorize(Policy = Permission.Order.ChooseProductForOrder)]
        public async Task<IActionResult> ChooseProductForOrder([FromBody] ChooseForOrderCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut]
        [Authorize(Policy = Permission.Order.CancelProductForOrder)]
        public async Task<IActionResult> CancelProductForOrder([FromBody] RemoveForOrderCommandRequest request)
        {
            var response=await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet] 
        [Authorize(Policy = Permission.Order.GetAllOrder)]
        public async Task<IActionResult> GetOrder([FromQuery] GetOrderQueryRequest request)
        {
            var response= await _mediator.Send(new GetOrderQueryRequest { Currency=request.Currency});
            return StatusCode((int)response.StatusCode,response);
        }

        [HttpGet]
        [Authorize(Policy = Permission.Order.GetPaidOrder)]
        public async Task<IActionResult> GetPaidOrder()
        {
            var response = await _mediator.Send(new GetPaidOrderQueryRequest());
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
