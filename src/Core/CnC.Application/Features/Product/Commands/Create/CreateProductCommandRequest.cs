using CnC.Application.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CnC.Application.Features.Product.Commands.Create;

public class CreateProductCommandRequest:IRequest<BaseResponse<string>>
{
    public string Name { get; set; } = null!;
    public string Model { get; set; } = null!;
    public decimal DiscountedPercent { get; set; }
    public decimal PriceAzn { get; set; }   
    public Guid CategoryId { get; set; }
    public IFormFile PreviewImageUrl { get; set; } = null!;
}
