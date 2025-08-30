using CnC.Application.Features.Category.MainCategory.Commands.Create;
using CnC.Application.Features.Category.MainCategory.Commands.Delete;
using CnC.Application.Features.Category.MainCategory.Commands.Update;
using CnC.Application.Features.Category.Queries.GetAll;
using CnC.Application.Features.Category.Queries.GetAllandName;
using CnC.Application.Features.Category.Queries.GetById;
using CnC.Application.Features.Category.SubCategory.Commands.Create;
using CnC.Application.Features.Category.SubCategory.Commands.Delete;
using CnC.Application.Features.Category.SubCategory.Commands.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CnC.WepApi.Controllers.Category
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMainCategory([FromBody] CreateMainCategoryCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubCategory([FromBody] CreateSubCategoryCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var response = await _mediator.Send(new CategoryGetAllQueryRequest());
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryById([FromQuery] CategoryGetIdQueryRequest request)
        {
            var response = await _mediator.Send(new CategoryGetIdQueryRequest { Id=request.Id});
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoriesByName([FromQuery] CategoryGetNameQueryRequest request)
        {
            var response = await _mediator.Send(new CategoryGetNameQueryRequest { Search=request.Search});
            return StatusCode((int)response.StatusCode, response);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateMainCategory([FromBody] UpdateMainCategoryCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSubCategory([FromBody] UpdateSubCategoryCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMainCategory([FromBody]DeleteMainCategoryCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSubCategory([FromBody]DeleteSubCategoryCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
