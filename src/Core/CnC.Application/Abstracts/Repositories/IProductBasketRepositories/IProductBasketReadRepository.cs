using CnC.Application.Abstracts.Repositories.IRepositories;
using CnC.Domain.Entities;

namespace CnC.Application.Abstracts.Repositories.IProductBasketRepositories;

public interface IProductBasketReadRepository:IReadRepository<ProductBasket>
{
    Task<ProductBasket?> ExistProductInBasket(Guid basketId,Guid productId,CancellationToken ct);

    Task<List<ProductBasket>> GetByBasketIdAsync(Guid basketId, CancellationToken ct);
}
