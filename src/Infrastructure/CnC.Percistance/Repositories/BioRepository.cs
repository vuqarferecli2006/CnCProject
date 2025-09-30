using CnC.Application.Abstracts.Repositories.IBioRepositories;
using CnC.Domain.Entities;
using CnC.Percistance.Contexts;

namespace CnC.Percistance.Repositories;

public class BioRepository : Repository<Bio>, IBioReadRepository, IBioWriteRepository
{
    public BioRepository(AppDbContext context) : base(context)
    {
    }
}
