using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.Category.Queries.GetAll;

public class CategoryGetAllQueryRequest:IRequest<BaseResponse<List<CategoryResponse>>>
{
}
