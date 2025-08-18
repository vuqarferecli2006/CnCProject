using CnC.Domain.Entities;
using System.Linq.Expressions;

namespace CnC.Application.Abstracts.Repositories.IRepositories;

public interface IReadRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id);

    IQueryable<T> GetAll(bool isTracking = false);

    IQueryable<T> GetByFiltered(Expression<Func<T, bool>> predicate,
                                Expression<Func<T, object>>[]? include=null,
                                bool isTracking=false);
    IQueryable<T> GetAllFiltered(Expression<Func<T,bool>> predicate,
                                Expression<Func<T, object>>[]? include=null,
                                Expression<Func<T,object>>? orderBy=null,
                                bool isOrderByAsc=true,
                                bool isTracking=false);
}
