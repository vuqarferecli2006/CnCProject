using CnC.Application.Abstracts.Repositories.IRepositories;
using CnC.Domain.Entities;

namespace CnC.Application.Abstracts.Repositories.IDownloadRepositories;

public interface IDownloadReadRepository:IReadRepository<Download>
{
    Task<List<Download>> GetByOrderProductIdAsync(Guid orderProductId, CancellationToken ct = default);
}
