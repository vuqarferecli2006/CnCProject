using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.Category.MainCategory.Commands.Delete;

public class DeleteMainCategoryCommandRequest:IRequest<BaseResponse<string>>
{
    public Guid Id { get; set; }
}
