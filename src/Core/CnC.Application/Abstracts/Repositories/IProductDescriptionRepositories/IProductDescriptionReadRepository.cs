using CnC.Application.Abstracts.Repositories.IRepositories;
using CnC.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CnC.Application.Abstracts.Repositories.IProductDescriptionRepository;

public interface IProductDescriptionReadRepository : IReadRepository<ProductDescription>
{
    Task<ProductDescription?> GetByIdWithFilesAsync(Guid id, CancellationToken cancellationToken);

    Task<ProductDescription?> GetProductDescriptionBySlugAsync(string slug, CancellationToken cancellationToken);
}
