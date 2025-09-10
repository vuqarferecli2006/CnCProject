using CnC.Application.Abstracts.Repositories.IRepositories;
using CnC.Domain.Entities;

namespace CnC.Application.Abstracts.Repositories.IProductViewRepositories;

public interface IProductViewReadRepository:IReadRepository<ProductView>
{
    Task<List<ProductView>> GetUserViewsAsync(string? userId, string? sessionId, CancellationToken cancellationToken);
    Task<List<ProductView>> GetBySessionIdAsync(string sessionId, CancellationToken ct);
    Task<List<ProductView>> GetByUserIdAsync(string userId, CancellationToken ct);
}
