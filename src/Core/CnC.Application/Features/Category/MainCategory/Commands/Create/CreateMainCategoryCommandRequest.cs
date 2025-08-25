using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.Category.MainCategory.Commands.Create;

public class CreateMainCategoryCommandRequest:IRequest<BaseResponse<string>>
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}
