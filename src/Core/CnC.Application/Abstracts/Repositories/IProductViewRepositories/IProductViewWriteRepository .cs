using CnC.Application.Abstracts.Repositories.IRepositories;
using CnC.Domain.Entities;

namespace CnC.Application.Abstracts.Repositories.IProductViewRepositories;

public interface IProductViewWriteRepository:IWriteRepository<ProductView>
{
    Task BulkDeleteAsync(IEnumerable<ProductView> views, CancellationToken ct);
    Task BulkAddAsync(IEnumerable<ProductView> views, CancellationToken ct);
}
