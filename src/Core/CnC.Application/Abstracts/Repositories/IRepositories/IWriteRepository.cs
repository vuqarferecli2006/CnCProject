using CnC.Domain.Entities;

namespace CnC.Application.Abstracts.Repositories.IRepositories;

public interface IWriteRepository<T> where T : BaseEntity
{
    Task AddAsync(T entity);

    void UpdateAsync(T entity);

    void DeleteAsync(T entity);

    Task SaveChangeAsync();
}
