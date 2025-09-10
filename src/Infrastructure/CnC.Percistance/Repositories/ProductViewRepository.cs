using CnC.Application.Abstracts.Repositories.IProductViewRepositories;
using CnC.Domain.Entities;
using CnC.Percistance.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CnC.Percistance.Repositories;

public class ProductViewRepository : Repository<ProductView>, IProductViewReadRepository, IProductViewWriteRepository
{
    private readonly AppDbContext _context;
    public ProductViewRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task BulkAddAsync(IEnumerable<ProductView> views, CancellationToken ct)
    {
        await _context.ProductViews.AddRangeAsync(views, ct);
    }

    public async Task BulkDeleteAsync(IEnumerable<ProductView> views, CancellationToken ct)
    {
        _context.ProductViews.RemoveRange(views);
        await Task.CompletedTask;
    }

    public async Task<List<ProductView>> GetBySessionIdAsync(string sessionId, CancellationToken ct)
    {
        return await _context.ProductViews
            .Include(p => p.Product)
            .Where(p => p.SessionId == sessionId)
            .OrderByDescending(p => p.ViewedAt)
            .ToListAsync(ct);
    }

    public async Task<List<ProductView>> GetByUserIdAsync(string userId, CancellationToken ct)
    {
        return await _context.ProductViews
            .Include(p => p.Product)
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.ViewedAt)
            .ToListAsync(ct);
    }

    public async Task<List<ProductView>> GetUserViewsAsync(string? userId, string? sessionId, CancellationToken cancellationToken)
    {
        return await _context.ProductViews
            .Where(p => (userId != null && p.UserId == userId) || (sessionId != null && p.SessionId == sessionId))
            .Include(p=>p.Product)
            .OrderBy(p => p.ViewedAt)
            .ToListAsync(cancellationToken);
    }
}
