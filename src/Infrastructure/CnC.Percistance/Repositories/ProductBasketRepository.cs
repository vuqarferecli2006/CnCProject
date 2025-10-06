using CnC.Application.Abstracts.Repositories.IProductBasketRepositories;
using CnC.Domain.Entities;
using CnC.Percistance.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CnC.Percistance.Repositories;

public class ProductBasketRepository : Repository<ProductBasket>, IProductBasketReadRepository, IProductBasketWriteRepository
{
    private readonly AppDbContext _context;

    public ProductBasketRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<ProductBasket?> ExistProductInBasket(Guid basketId, Guid productId, CancellationToken ct)
    {
        return await _context.ProductBaskets
            .Include(pb=>pb.Product)
            .FirstOrDefaultAsync(pb=>pb.BasketId==basketId&&pb.ProductId==productId,ct);
    }

    public async Task<List<ProductBasket>> GetByBasketIdAsync(Guid basketId, CancellationToken ct)
    {
        return await _context.ProductBaskets
            .Include(pb => pb.Product)
                .ThenInclude(p => p.ProductDescription)
            .Where(pb => pb.BasketId == basketId && pb.Product != null && !pb.Product.IsDeleted)
            .ToListAsync(ct);
    }

}
