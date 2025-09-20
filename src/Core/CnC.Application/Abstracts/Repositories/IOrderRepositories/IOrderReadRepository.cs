using CnC.Application.Abstracts.Repositories.IRepositories;
using CnC.Domain.Entities;

namespace CnC.Application.Abstracts.Repositories.IOrderRepositories;

public interface IOrderReadRepository:IReadRepository<Order>
{
    Task<OrderProduct?> GetUserActiveOrderProductAsync(string userId, Guid productId, CancellationToken ct);
    Task<Order?> GetUserActiveOrderAsync(string userId, CancellationToken ct);
    Task<Order?> GetOrderWithProductsAsync(Guid orderId, CancellationToken ct);
    Task<List<Order>> GetPaidOrdersByUserIdAsync(string userId, CancellationToken ct);
    Task<List<Order>> GetPaidOrdersWithProductsAsync(string userId, CancellationToken cancellationToken = default);
}
