using CnC.Application.Abstracts.Repositories.ICategoryRepositories;
using CnC.Domain.Entities;
using CnC.Percistance.Contexts;

namespace CnC.Percistance.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryReadRepository, ICategoryWriteRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }
}
