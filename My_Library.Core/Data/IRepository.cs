using System;
using System.Linq;
using System.Linq.Expressions;

namespace My_Library.Core.Data
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);
        TEntity GetById(int id);

        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Delete(int id);

        void MarkAsDelete(TEntity entity);
        void MarkAsModified(TEntity entity);
        void MarkAsNewOrModified(TEntity entity, int id);
    }
}
