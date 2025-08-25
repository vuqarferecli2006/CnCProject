using CnC.Application.Abstracts.Repositories.ICategoryRepositories;
using CnC.Application.Shared.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CnC.Application.Features.Category.SubCategory.Commands.Update;

public class UpdateSubCategoryCommandHandler : IRequestHandler<UpdateSubCategoryCommandRequest, BaseResponse<CategoryUpdateResponse>>
{
    private readonly ICategoryReadRepository _categoryReadRepository;
    private readonly ICategoryWriteRepository _categoryWriteRepository;

    public UpdateSubCategoryCommandHandler(ICategoryReadRepository categoryReadRepository,
                                         ICategoryWriteRepository categoryWriteRepository)
    {
        _categoryReadRepository = categoryReadRepository;
        _categoryWriteRepository = categoryWriteRepository;
    }

    public async Task<BaseResponse<CategoryUpdateResponse>> Handle(UpdateSubCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        var category =await _categoryReadRepository.GetByIdAsync(request.Id);

        if (category is null)
            return new("Category not found", HttpStatusCode.NotFound);

        if (category.ParentCategory is not null)
            return new("Only sub categories can be updated with this method", HttpStatusCode.BadRequest);

        if(request.NewParentCategoryId==request.Id)
            return new("A category cannot be its own parent", HttpStatusCode.BadRequest);

        var parentExists = await _categoryReadRepository.GetByFiltered(c =>
        c.Id == request.NewParentCategoryId &&
        c.ParentCategory == null).AnyAsync(cancellationToken);

        if (!parentExists)
            return new("New category not found", HttpStatusCode.BadRequest);

        var nameExists = await _categoryReadRepository.GetByFiltered(c =>
        c.Id != request.Id &&
        c.Name.ToLower().Trim() == request.Name.ToLower().Trim() &&
        c.ParentCategoryId == request.NewParentCategoryId).
        AnyAsync(cancellationToken);

        if (nameExists)
            return new("Another subcategory with this name already exists under the selected parent", HttpStatusCode.BadRequest);

        category.Name = request.Name.Trim();
        category.Description = request.Description?.Trim();
        category.ParentCategoryId = request.NewParentCategoryId;

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
