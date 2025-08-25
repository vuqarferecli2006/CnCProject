using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.Category.SubCategory.Commands.Create;

public class CreateSubCategoryCommandRequest:IRequest<BaseResponse<string>>
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Guid ParentCategoryId { get; set; }
}
