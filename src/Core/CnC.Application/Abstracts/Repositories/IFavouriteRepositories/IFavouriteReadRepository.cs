using CnC.Application.Abstracts.Repositories.IRepositories;
using CnC.Domain.Entities;

namespace CnC.Application.Abstracts.Repositories.IFavouriteRepositories;

public interface IFavouriteReadRepository:IReadRepository<BookMark>
{
    Task<bool> IsProductFavouriteAsync(Guid productId, string userId);

    Task<BookMark?> GetFavouriteAsync(Guid productId,string userId);

    Task<List<BookMark>> GetUserFavouritesAsync(string userId, CancellationToken cancellationToken = default);
}
