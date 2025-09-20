using CnC.Application.Abstracts.Repositories.IFavouriteRepositories;
using CnC.Application.Shared.Responses;
using CnC.Domain.Entities;
using CnC.Percistance.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CnC.Percistance.Repositories;

public class FavouriteRepository : Repository<BookMark>, IFavouriteReadRepository, IFavouriteWriteRepository
{
    private readonly AppDbContext _context;
    public FavouriteRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<BookMark?> GetFavouriteAsync(Guid productId, string userId)
    {
        return await _context.BookMarks.FirstOrDefaultAsync(b=>b.ProductId == productId && b.UserId == userId);
    }

    public async Task<bool> IsProductFavouriteAsync(Guid productId, string userId)
    {
        return await _context.BookMarks.AnyAsync(b=>b.ProductId == productId && b.UserId == userId);
    }

    public async Task<List<BookMark>> GetUserFavouritesAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _context.BookMarks
                             .Where(b => b.UserId == userId)
                             .ToListAsync(cancellationToken);
    }

}
