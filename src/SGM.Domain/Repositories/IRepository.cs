using System.Linq.Expressions;

namespace SGM.Domain.Repositories;

/// <summary>
/// Generic repository interface
/// </summary>
public interface IRepository<TEntity> where TEntity: class, IAggregateRoot
{
    Task<TEntity> GetByIdAsync(object id);
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);
    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null);
    IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> predicate = null);
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteByIdAsync(object id);
    Task DeleteAsync(TEntity entity);
}
