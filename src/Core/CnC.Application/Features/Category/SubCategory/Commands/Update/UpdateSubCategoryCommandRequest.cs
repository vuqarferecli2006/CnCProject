using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.Category.SubCategory.Commands.Update;

public class UpdateSubCategoryCommandRequest:IRequest<BaseResponse<CategoryUpdateResponse>>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Guid NewParentCategoryId { get; set; }
}
