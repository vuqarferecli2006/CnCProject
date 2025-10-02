using CnC.Application.Features.Favourite.Commands.Create;
using CnC.Application.Features.Favourite.Commands.Delete;
using CnC.Application.Features.Favourite.Queries;
using CnC.Application.Shared.Permissions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CnC.WepApi.Controllers.Favourite
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FavouritesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FavouritesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Policy =Permission.Favourite.AddProductFavourite)]
        public async Task<IActionResult> AddProductToFavourite([FromBody] CreateFavouriteCommandRequest request)
        {
            var response=await _mediator.Send(request);
            return StatusCode((int)response.StatusCode,response);
        }

        [HttpDelete]
        [Authorize(Policy = Permission.Favourite.RemoveProductFavourite)]
        public async Task<IActionResult> DeleteProductFromFavourite([FromQuery] DeleteFavouriteCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode,response);
        }

        [HttpGet]
        [Authorize(Policy =Permission.Favourite.GetAllProductFavourite)]
        public async Task<IActionResult> GetProductInFavourite([FromQuery] GetAllFavouriteQuerisRequest request)
        {
            var response=await _mediator.Send(new GetAllFavouriteQuerisRequest {Currency=request.Currency });
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
