using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.Category.MainCategory.Commands.Update;

public class UpdateMainCategoryCommandRequest:IRequest<BaseResponse<CategoryUpdateResponse>>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}
