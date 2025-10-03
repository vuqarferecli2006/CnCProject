using CnC.Application.Features.Payment.Commands;
using CnC.Application.Features.PaymentMethod;
using CnC.Application.Features.PaymentMethod.Queries;
using CnC.Application.Shared.Permissions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CnC.WepApi.Controllers.Payment
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Policy = Permission.Payment.ChoosePaymentMethod)]
        public async Task<IActionResult> ChoosePaymentMethod([FromQuery]CreatePaymentMethodCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost]
        [Authorize(Policy = Permission.Payment.PaymentCreate)]
        public async Task<IActionResult> PaymentForTest([FromQuery]CreatePaymentCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPaymentMethod()
        {
            var response = await _mediator.Send(new GetPaymentMethodQueryRequest());
            return StatusCode((int)response.StatusCode, response);
        }

    }
}
