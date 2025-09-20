using CnC.Application.Abstracts.Repositories.ICategoryRepositories;
using CnC.Application.Shared.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CnC.Application.Features.Category.Queries.GetById;

public class CategoryGetIdQueryHandler : IRequestHandler<CategoryGetIdQueryRequest, BaseResponse<CategoryResponse>>
{
    private readonly ICategoryReadRepository _categoryReadRepository;

    public CategoryGetIdQueryHandler(ICategoryReadRepository categoryReadRepository)
    {
        _categoryReadRepository = categoryReadRepository;
    }

    public async Task<BaseResponse<CategoryResponse>> Handle(CategoryGetIdQueryRequest request, CancellationToken cancellationToken)
    {
        var category = await _categoryReadRepository.GetCategoryWithSubcategoriesBySlugAsync(request.Slug, cancellationToken);

        if (category is null)
            return new("Category not found", false, HttpStatusCode.NotFound);

        var dto = MapCategoryRecursive(category);

        return new("Success", dto, true, HttpStatusCode.OK);

    }
   
    private CategoryResponse MapCategoryRecursive(Domain.Entities.Category category)
    {
        return new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug,
            Description = category.Description,
            SubCategories = category.SubCategories
            .Where(sc => !sc.IsDeleted)
            .Select(sc => new CategorySubResponse
            {
                Id = sc.Id,
                Name = sc.Name,
                Description = sc.Description,
                SubCategories = sc.SubCategories
                    .Where(ssc => !ssc.IsDeleted)
                    .Select(ssc => new CategorySubResponse
                    {
                        Id = ssc.Id,
                        Name = ssc.Name,
                        Description = ssc.Description
                    }).ToList()
            }).ToList()
        };
    }
}
