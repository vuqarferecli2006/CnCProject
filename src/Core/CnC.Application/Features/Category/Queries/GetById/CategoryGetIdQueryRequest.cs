using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.Category.Queries.GetById;

public class CategoryGetIdQueryRequest: IRequest<BaseResponse<CategoryResponse>>
{
    public Guid Id { get; set; }
}
