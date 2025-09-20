using CnC.Application.Abstracts.Repositories.IRepositories;
using CnC.Domain.Entities;

namespace CnC.Application.Abstracts.Repositories.IBasketRepositories;

public interface IBasketReadRepository:IReadRepository<Basket>
{
    Task<Basket?> GetByBasketUser(string userId,CancellationToken ct);
    Task<Basket?> GetUserBasketWithProductsAsync(string userId, CancellationToken ct);
}
