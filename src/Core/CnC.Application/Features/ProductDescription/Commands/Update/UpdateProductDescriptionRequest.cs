using CnC.Application.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CnC.Application.Features.ProductDescription.Commands.Update;

public class UpdateProductDescriptionRequest:IRequest<BaseResponse<string>> 
{
    public Guid DescriptionId { get; set; }

    public string Description { get; set; } = null!;

    public string Model { get; set; } = null!;

    public IFormFile ImageUrls { get; set; }=null!;

    public List<IFormFile> FileUrls { get; set; } = new();
}
