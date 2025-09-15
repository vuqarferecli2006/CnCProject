using CnC.Application.Abstracts.Repositories.IOrderRepositories;
using CnC.Domain.Entities;
using CnC.Percistance.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace CnC.Percistance.Repositories;

public class OrderRepository : Repository<Order>, IOrderReadRepository, IOrderWriteRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<OrderProduct?> GetUserActiveOrderProductAsync(string userId, Guid productId, CancellationToken ct)
    {
        var orderProduct = await _context.Orders
            .Where(o => o.UserId == userId && !o.isPaid)
            .SelectMany(o => o.OrderProducts)
            .FirstOrDefaultAsync(op => op.ProductId == productId, ct);

        return orderProduct;
    }

    public async Task<Order?> GetUserActiveOrderAsync(string userId, CancellationToken ct)
    {
        return await _context.Orders
            .Where(o => o.UserId == userId && !o.isPaid)
            .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
            .AsNoTracking()
            .FirstOrDefaultAsync(ct);

    }
}
