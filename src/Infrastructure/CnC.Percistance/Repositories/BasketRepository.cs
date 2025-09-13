using CnC.Application.Abstracts.Repositories.IBasketRepositories;
using CnC.Domain.Entities;
using CnC.Percistance.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CnC.Percistance.Repositories;

public class BasketRepository : Repository<Basket>, IBasketReadRepository, IBasketWriteRepository
{
    private readonly AppDbContext _context;
    public BasketRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Basket?> GetByBasketUser(string userId,CancellationToken ct)
    {
        return await _context.Baskets.FirstOrDefaultAsync(b => b.UserId == userId,ct);
    }
}
