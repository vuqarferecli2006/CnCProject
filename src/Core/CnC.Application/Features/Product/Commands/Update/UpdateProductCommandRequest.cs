using CnC.Application.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CnC.Application.Features.Product.Commands.Update;

public class UpdateProductCommandRequest : IRequest<BaseResponse<string>>
{
    public Guid ProductId { get; set; }

    public string Name { get; set; } = null!;

    public decimal DiscountedPercent { get; set; }

    public decimal PriceAzn { get; set; }

    public IFormFile? PreviewImageUrl { get; set; }

    public Guid NewCategoryId { get; set; }
}
