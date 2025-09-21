using CnC.Application.Shared.Responses;
using CnC.Domain.Enums;
using MediatR;

namespace CnC.Application.Features.ProductDescription.Queries.GetByIdDescription;

public class GetBySlugDescriptionQueryRequest:IRequest<BaseResponse<ProductDescriptionResponse>>
{
    public string Slug { get; set; } = null!;

    public Currency Currency { get; set; } = Currency.AZN;
}
    