using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.Category.Queries.GetAllandName;

public class CategoryGetNameQueryRequest:IRequest<BaseResponse<List<CategoryResponse>>>
{
    public string? Search { get; set; }
}
