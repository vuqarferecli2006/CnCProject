using CnC.Application.Abstracts.Repositories.IInformationModelRepositories;
using CnC.Domain.Entities;
using CnC.Percistance.Contexts;

namespace CnC.Percistance.Repositories;

public class InformationModelRepository : Repository<InformationModel>, IInformationModelReadRepository, IInformationModelWriteRepository
{
    public InformationModelRepository(AppDbContext context) : base(context)
    {
    }
}
