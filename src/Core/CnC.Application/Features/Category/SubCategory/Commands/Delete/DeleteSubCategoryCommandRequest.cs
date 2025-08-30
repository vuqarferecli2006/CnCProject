using CnC.Application.Shared.Responses;
using MediatR;

namespace CnC.Application.Features.Category.SubCategory.Commands.Delete;

public class DeleteSubCategoryCommandRequest:IRequest<BaseResponse<string>>
{
    public Guid Id { get; set; }
}
