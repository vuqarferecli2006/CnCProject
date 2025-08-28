using CnC.Application.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CnC.Application.Features.ProductDescription.Commands.Create;

public class CreateProductDescriptionRequest:IRequest<BaseResponse<string>>
{
    public Guid ProductId { get; set; }

    public string Description { get; set; } = null!;

    public string Model { get; set; } = null!;

    public IFormFile ImageUrl { get; set; } = null!;

    public List<IFormFile> FileUrl { get; set; } = new();
}
