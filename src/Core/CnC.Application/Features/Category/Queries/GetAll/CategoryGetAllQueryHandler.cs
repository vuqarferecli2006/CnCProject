using CnC.Application.Abstracts.Repositories.ICategoryRepositories;
using CnC.Application.Features.Category.Queries.GetAllandName;
using CnC.Application.Shared.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CnC.Application.Features.Category.Queries.GetAll;

public class CategoryGetAllQueryHandler : IRequestHandler<CategoryGetAllQueryRequest, BaseResponse<List<CategoryResponse>>>
{
    private readonly ICategoryReadRepository _categoryReadRepository;
    public CategoryGetAllQueryHandler(ICategoryReadRepository categoryReadRepository)
    {
        _categoryReadRepository = categoryReadRepository;
    }

    public async Task<BaseResponse<List<CategoryResponse>>> Handle(CategoryGetAllQueryRequest request, CancellationToken cancellationToken)
    {
        var categories = await _categoryReadRepository
            .GetAll()
            .Where(c => c.ParentCategoryId == Guid.Empty || c.ParentCategoryId == null && !c.IsDeleted)
            .Include(c => c.SubCategories.Where(sc => !sc.IsDeleted))
                .ThenInclude(sc => sc.SubCategories.Where(ssc => !ssc.IsDeleted))
            .ToListAsync(cancellationToken);

        if (!categories.Any())
            return new("No categories found",HttpStatusCode.NotFound);

        var dtos = categories.Select(c => MapCategoryRecursive(c)).ToList();

        return new("Success", dtos, true, HttpStatusCode.OK);

    }
   
    private CategoryResponse MapCategoryRecursive(Domain.Entities.Category category)
    {
        return new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
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
