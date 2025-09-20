using CnC.Application.Abstracts.Repositories.ICategoryRepositories;
using CnC.Domain.Entities;
using CnC.Percistance.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CnC.Percistance.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryReadRepository, ICategoryWriteRepository
{
    private readonly AppDbContext _context;
    public CategoryRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Category?> GetCategoryWithSubcategoriesBySlugAsync(string slug, CancellationToken cancellationToken)
    {
        return await _context.Categories
           .Where(c => c.Slug == slug && !c.IsDeleted)
           .Include(c => c.SubCategories.Where(sc => !sc.IsDeleted))
               .ThenInclude(sc => sc.SubCategories.Where(ssc => !ssc.IsDeleted))
           .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Category>> GetParentCategoriesWithSubcategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .Where(c => (c.ParentCategoryId == Guid.Empty || c.ParentCategoryId == null) && !c.IsDeleted)
            .Include(c => c.SubCategories.Where(sc => !sc.IsDeleted))
                .ThenInclude(sc => sc.SubCategories.Where(ssc => !ssc.IsDeleted))
            .ToListAsync(cancellationToken);
    }
}
