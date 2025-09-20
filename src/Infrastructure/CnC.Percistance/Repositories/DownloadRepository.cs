using CnC.Application.Abstracts.Repositories.IDownloadRepositories;
using CnC.Domain.Entities;
using CnC.Percistance.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CnC.Percistance.Repositories;

public class DownloadRepository : Repository<Download>, IDownloadReadRepository, IDownloadWriteRepository
{
    private readonly AppDbContext _context;
    
    public DownloadRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Download>> GetByOrderProductIdAsync(Guid orderProductId, CancellationToken ct)
    {
        return await _context.Downloads
             .Where(d => d.OrderProductId == orderProductId)
             .ToListAsync(ct);
    }
}
