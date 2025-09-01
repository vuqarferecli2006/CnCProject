using CnC.Domain.Entities;

namespace CnC.Application.Abstracts.Repositories.IRepositories;

public interface IWriteRepository<T> where T : BaseEntity
{
    Task AddAsync(T entity);

    void Update(T entity);

    void Delete(T entity);

    void DeleteRange(IEnumerable<T> entities);

    Task SaveChangeAsync();
}
