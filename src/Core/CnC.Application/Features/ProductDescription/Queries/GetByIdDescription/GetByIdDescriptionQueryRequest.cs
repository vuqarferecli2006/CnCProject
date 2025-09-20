using CnC.Application.Shared.Responses;
using CnC.Domain.Enums;
using MediatR;

namespace CnC.Application.Features.ProductDescription.Queries.GetByIdDescription;

public class GetByIdDescriptionQueryRequest:IRequest<BaseResponse<ProductDescriptionResponse>>
{
    public Guid ProductId { get; set; }

    public Currency Currency { get; set; } = Currency.AZN;
}
    