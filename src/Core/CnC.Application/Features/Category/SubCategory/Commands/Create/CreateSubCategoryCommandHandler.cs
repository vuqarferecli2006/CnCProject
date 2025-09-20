using CnC.Application.Abstracts.Repositories.ICategoryRepositories;
using CnC.Application.Shared.Helpers.SlugHelpers;
using CnC.Application.Shared.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CnC.Application.Features.Category.SubCategory.Commands.Create;

public class CreateSubCategoryCommandHandler : IRequestHandler<CreateSubCategoryCommandRequest, BaseResponse<string>>
{
    private readonly ICategoryWriteRepository _categoryWriteRepository;
    private readonly ICategoryReadRepository _categoryReadRepository;

    public CreateSubCategoryCommandHandler(ICategoryWriteRepository categoryWriteRepository, ICategoryReadRepository categoryReadRepository)
    {
        _categoryWriteRepository = categoryWriteRepository;
        _categoryReadRepository = categoryReadRepository;
    }

    public async Task<BaseResponse<string>> Handle(CreateSubCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        var parentCategory= await _categoryReadRepository.GetByFiltered(c=>c.Id==request.ParentCategoryId).AnyAsync(cancellationToken);
        if (!parentCategory)
            return new("Parent category not found",HttpStatusCode.NotFound);

        var exists=await _categoryReadRepository
            .GetByFiltered(c => c.ParentCategoryId == request.ParentCategoryId 
                                &&!c.IsDeleted
                                &&c.Name.Trim().ToLower() == request.Name.Trim().ToLower()).AnyAsync(cancellationToken);

        if (exists)
            return new("This sub category already exists under the specified parent category",HttpStatusCode.BadRequest);

        var category = new Domain.Entities.Category
        {
            Name = request.Name,
            Slug = SlugHelper.GenerateSlug(request.Name),
            Description = request.Description,
            ParentCategoryId = request.ParentCategoryId
        };

        await _categoryWriteRepository.AddAsync(category);
        await _categoryWriteRepository.SaveChangeAsync();
        
        return new("Sub category created successfully",true,HttpStatusCode.Created);
    }
}
