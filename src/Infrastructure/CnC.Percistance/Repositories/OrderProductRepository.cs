using CnC.Application.Abstracts.Repositories.IOrderProductRepositories;
using CnC.Domain.Entities;
using CnC.Percistance.Contexts;

namespace CnC.Percistance.Repositories;

public class OrderProductRepository : Repository<OrderProduct>, IOrderProductReadRepository, IOrderProductWriteRepository
{
    public OrderProductRepository(AppDbContext context) : base(context)
    {
    }
}
