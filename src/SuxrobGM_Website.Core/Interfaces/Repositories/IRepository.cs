using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SuxrobGM_Website.Core.Interfaces.Entities;

namespace SuxrobGM_Website.Core.Interfaces.Repositories
{
    public interface IRepository
    {
        Task<TEntity> GetByIdAsync<TEntity>(string id) where TEntity: class, IEntity<string>;
        Task<TEntity> GetAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity: class, IEntity<string>;
        Task<List<TEntity>> GetListAsync<TEntity>() where TEntity: class, IEntity<string>;
        Task<List<TEntity>> GetListAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity: class, IEntity<string>;
        IQueryable<TEntity> GetAll<TEntity>(Expression<Func<TEntity, bool>> predicate = null) where TEntity: class, IEntity<string>;
        Task<TEntity> AddAsync<TEntity>(TEntity entity) where TEntity: class, IEntity<string>;
        Task UpdateAsync<TEntity>(TEntity entity) where TEntity: class, IEntity<string>;
        Task DeleteAsync<TEntity>(TEntity entity) where TEntity: class, IEntity<string>;
    }
}
