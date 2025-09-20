using CnC.Domain.Entities;

namespace CnC.Application.Abstracts.Repositories.IRepositories;

public interface IWriteRepository<T> where T : BaseEntity
{
    Task AddAsync(T entity);

    Task AddRangeAsync(IEnumerable<T> entities);

    void Update(T entity);

    void UpdateRange(IEnumerable<T> entities);
    
    void Delete(T entity);

    void DeleteRange(IEnumerable<T> entities);

    Task SaveChangeAsync();
}
