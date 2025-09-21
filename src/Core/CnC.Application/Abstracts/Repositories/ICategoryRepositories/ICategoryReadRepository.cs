using CnC.Application.Abstracts.Repositories.IRepositories;
using CnC.Domain.Entities;

namespace CnC.Application.Abstracts.Repositories.ICategoryRepositories;

public interface ICategoryReadRepository:IReadRepository<Category>
{
    Task<Category?> GetCategoryWithSubcategoriesBySlugAsync(string slug, CancellationToken cancellationToken);

    Task<List<Category>> GetParentCategoriesWithSubcategoriesAsync(CancellationToken cancellationToken);
}
