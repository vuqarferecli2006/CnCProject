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
        var orderProduct = await _context.OrderProducts
            .Include(op=>op.Order)
            .Where(op => op.ProductId == productId && op.Order.UserId == userId && !op.Order.isPaid)
            .AsNoTracking()
            .FirstOrDefaultAsync(op => op.ProductId == productId, ct);

        return orderProduct;
    }


    public async Task<Order?> GetUserActiveOrderAsync(string userId, CancellationToken ct)
    {
        return await _context.Orders
            .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                    .ThenInclude(p => p.ProductDescription) 
            .FirstOrDefaultAsync(o => o.UserId == userId && !o.isPaid, ct);
    }


    public async Task<Order?> GetOrderWithAllDetailsAsync(Guid orderId, CancellationToken ct)
    {
        return await _context.Orders
            .Include(o => o.User) // Alıcı
            .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                    .ThenInclude(p => p.User) // Satıcı
            .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                    .ThenInclude(p => p.ProductDescription)
                        .ThenInclude(pd => pd.ProductFiles)
            .FirstOrDefaultAsync(o => o.Id == orderId, ct);
    }

    public async Task<List<Order>> GetPaidOrdersByUserIdAsync(string userId, CancellationToken ct)
    {
        return await _context.Orders
            .Where(o => o.UserId == userId && o.isPaid)
            .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                    .ThenInclude(p => p.ProductDescription)
            .Include(o => o.Payment)
                 .ThenInclude(p=>p.PaymentMethod)
            .ToListAsync(ct);
    }

    public async Task<List<Order>> GetPaidOrdersWithProductsAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Where(o => o.UserId == userId && o.isPaid)
            .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                    .ThenInclude(p => p.ProductDescription)
                        .ThenInclude(pd => pd.ProductFiles)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

}
