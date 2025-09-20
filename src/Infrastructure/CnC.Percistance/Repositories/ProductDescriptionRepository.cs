using CnC.Application.Abstracts.Repositories.IProductDescriptionRepository;
using CnC.Domain.Entities;
using CnC.Percistance.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CnC.Percistance.Repositories;

public class ProductDescriptionRepository : Repository<ProductDescription>, IProductDescriptionWriteRepository, IProductDescriptionReadRepository
{
    private readonly AppDbContext _context; 
    public ProductDescriptionRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<ProductDescription?> GetByIdWithFilesAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.ProductDescriptions
            .Include(pd => pd.ProductFiles)
            .FirstOrDefaultAsync(pd => pd.Id == id, cancellationToken);
    }

    public async Task<ProductDescription?> GetProductDescriptionByIdAsync(Guid productId, CancellationToken cancellationToken)
    {
        return await _context.ProductDescriptions
            .Include(pd => pd.Product)
            .ThenInclude(p => p.ProductCurrencies)
            .FirstOrDefaultAsync(pd => pd.ProductId == productId, cancellationToken);
    }
}
