using CnC.Application.Abstracts.Repositories.ICategoryRepositories;
using CnC.Application.Shared.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CnC.Application.Features.Category.MainCategory.Commands.Update;

public class UpdateMainCategoryCommandHandler : IRequestHandler<UpdateMainCategoryCommandRequest, BaseResponse<CategoryUpdateResponse>>
{
    private readonly ICategoryReadRepository _categoryReadRepository;
    private readonly ICategoryWriteRepository _categoryWriteRepository;

    public UpdateMainCategoryCommandHandler(ICategoryReadRepository categoryReadRepository,
                                        ICategoryWriteRepository categoryWriteRepository)
    {
        _categoryReadRepository = categoryReadRepository;
        _categoryWriteRepository = categoryWriteRepository;
    }

    public async Task<BaseResponse<CategoryUpdateResponse>> Handle(UpdateMainCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        var category = await _categoryReadRepository.GetByIdAsync(request.Id);

        if (category is null)
            return new("Category not found", HttpStatusCode.NotFound);

        if (category.ParentCategory is not null)
            return new("Only main categories can be updated with this method", HttpStatusCode.BadRequest);

        var nameExists = await _categoryReadRepository.GetByFiltered(c =>
        c.Id != request.Id &&
        c.Name.ToLower().Trim() == request.Name.ToLower().Trim() &&
        c.ParentCategory == null).AnyAsync(cancellationToken);

        if (nameExists)
            return new("Another main category with this name already exists", HttpStatusCode.BadRequest);

        category.Name = request.Name.Trim();
        category.Description = request.Description?.Trim();


        _categoryWriteRepository.Update(category);
        await _categoryWriteRepository.SaveChangeAsync();

        var response = new CategoryUpdateResponse
        {
            Name = category.Name,
            Description = category.Description,
            ParentCategoryId = category.ParentCategoryId
        };

        return new(response, true, HttpStatusCode.OK);
    }
}
