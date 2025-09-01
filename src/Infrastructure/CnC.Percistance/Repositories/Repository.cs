using CnC.Application.Abstracts.Repositories.IRepositories;
using CnC.Domain.Entities;
using CnC.Percistance.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CnC.Percistance.Repositories;

public class Repository<T> : IReadRepository<T>, IWriteRepository<T> where T : BaseEntity, new()
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> Table;

    public Repository(AppDbContext context)
    {
        _context = context;
        Table = _context.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        await Table.AddAsync(entity);
    }

    public void Update(T entity)
    {
        Table.Update(entity);
    }
    
    public void Delete(T entity)
    {
        Table.Remove(entity);
    }

    public void DeleteRange(IEnumerable<T> entities)
    {
        Table.RemoveRange(entities);
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
       return await Table.FindAsync(id);
    }

    public IQueryable<T> GetAll(bool isTracking = false)
    {
        if(!isTracking)
            return Table.AsNoTracking();
        return Table;
    }

    public IQueryable<T> GetByFiltered(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? include = null, bool isTracking = false)
    {
        IQueryable<T> query = Table;

        if(predicate is not null)
            query = query.Where(predicate);

        if(include is not null)
            foreach(var includeExpression in include)
                query=query.Include(includeExpression);
      
        if (!isTracking)
            return query.AsNoTracking();
       
        return query;
           
    }
    
    public IQueryable<T> GetAllFiltered(Expression<Func<T, bool>> predicate, 
                                    Expression<Func<T, object>>[]? include = null, 
                                    Expression<Func<T, object>>? orderBy = null, 
                                    bool isOrderByAsc = true, 
                                    bool isTracking = false)
    {
        IQueryable<T> query = Table;
        
        if(predicate is not null)
            query = query.Where(predicate);
        
        if(include is not null)
            foreach(var includeExpression in include)
                query=query.Include(includeExpression);
        
        if(orderBy is not null)
        {
            if(isOrderByAsc)
                query = query.OrderBy(orderBy);
            else
                query = query.OrderByDescending(orderBy);
        }

        if (!isTracking)
            return query.AsNoTracking();

        return query;
    }

    public async Task SaveChangeAsync()
    {
       await _context.SaveChangesAsync();
    }
}
