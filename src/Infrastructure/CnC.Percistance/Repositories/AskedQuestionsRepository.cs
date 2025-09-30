using CnC.Application.Abstracts.Repositories.IAskedQuestionsRepositories;
using CnC.Domain.Entities;
using CnC.Percistance.Contexts;

namespace CnC.Percistance.Repositories;

public class AskedQuestionsRepository : Repository<FreequentlyAskedQuestion>, IWriteAskedQuestionsRepository, IReadAskedQuestionsRepository
{
    public AskedQuestionsRepository(AppDbContext context) : base(context)
    {
    }
}
