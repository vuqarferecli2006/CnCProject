using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.Category.Queries.GetById;

public class CategoryGetIdQueryRequest: IRequest<BaseResponse<CategoryResponse>>
{
    public string Slug { get; set; } = null!;
}
