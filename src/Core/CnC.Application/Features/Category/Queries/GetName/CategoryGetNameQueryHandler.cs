using CnC.Application.Abstracts.Repositories.ICategoryRepositories;
using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CnC.Application.Features.Category.Queries.GetAllandName;

public class CategoryGetNameQueryHandler : IRequestHandler<CategoryGetNameQueryRequest, BaseResponse<List<CategoryResponse>>>
{
    private readonly ICategoryReadRepository _categoryReadRepository;
    public CategoryGetNameQueryHandler(ICategoryReadRepository categoryReadRepository)
    {
        _categoryReadRepository = categoryReadRepository;
    }

    public async Task<BaseResponse<List<CategoryResponse>>> Handle(CategoryGetNameQueryRequest request, CancellationToken cancellationToken)
    {
        // DB query
        var categories = await _categoryReadRepository
            .GetAll(isTracking: false)
            .Where(c => c.Name.ToLower().Contains(request.Search!.Trim().ToLower()) && !c.IsDeleted)
            .Include(c => c.SubCategories.Where(sc => !sc.IsDeleted))
                .ThenInclude(sc => sc.SubCategories.Where(scc => !scc.IsDeleted))
            .ToListAsync(cancellationToken);

        if (!categories.Any())
            return new("No categories found matching the search criteria", false, HttpStatusCode.NotFound);

        var result = categories.Select(c => MapCategory(c)).ToList();

        return new BaseResponse<List<CategoryResponse>>(result, true, HttpStatusCode.OK);

    }
    private CategoryResponse MapCategory(Domain.Entities.Category category)
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
