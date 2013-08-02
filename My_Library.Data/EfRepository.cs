using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using My_Library.Core.Data;

namespace My_Library.Data
{
    /// <summary>
    /// The EF-dependent, generic repository for data access
    /// </summary>
    /// <typeparam name="TEntity">Type of entity for this Repository.</typeparam>
    public class EfRepository<TEntity>
        : IRepository<TEntity> where TEntity : class
    {

        public EfRepository(IUnitOfWork uow)
        {
            if (uow == null) throw new ArgumentNullException("uow");

            Uow = uow as EfUnitOfWork;
        }

        //public EfRepository(IUnitOfWork uow)
        //{
        //    if (uow == null) 
        //        throw new ArgumentNullException("uow");
        //    var efUow = uow as EfUnitOfWork;
        //    if (efUow == null)
        //        throw new NullReferenceException("uow is not ef unit of work");
        //    if (efUow.Context == null)
        //        throw new NullReferenceException("uow.Context");

        //    Uow = efUow;
        //    DbContext = efUow.Context;

        //    DbSet = DbContext.Set<T>();
        //}

        protected EfUnitOfWork Uow { get; set; }
        protected DbContext DbContext
        {
            get { return Uow.Context; }
        }
        protected DbSet<TEntity> DbSet
        {

            get { return Uow.Context.Set<TEntity>(); }
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return DbSet;
        }

        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable = GetAll();
            foreach (Expression<Func<TEntity, object>> includeProperty in includeProperties)
            {

                queryable = queryable.Include(includeProperty);
            }

            return queryable;
        }

        public virtual TEntity GetById(int id)
        {
            //return DbSet.FirstOrDefault(PredicateBuilder.GetByIdPredicate<T>(id));
            return DbSet.Find(id);
        }


        public virtual void Add(TEntity entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                DbSet.Add(entity);
            }
        }

        public virtual void Update(TEntity entity)
        {
            DbEntityEntry dbEntityEntry = GetDbEntityEntrySafely(entity);
            dbEntityEntry.State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity)
        {
            DbEntityEntry dbEntityEntry = GetDbEntityEntrySafely(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }

        public virtual void Delete(int id)
        {
            var entity = GetById(id);
            if (entity == null) return; // not found; assume already deleted.
            Delete(entity);
        }

        public void MarkAsDelete(TEntity entity)
        {
            DbContext.Entry(entity).State = EntityState.Deleted;
        }

        public void MarkAsModified(TEntity entity)
        {
            DbEntityEntry dbEntityEntry = GetDbEntityEntrySafely(entity);
            dbEntityEntry.State = EntityState.Modified;
        }

        public void MarkAsNewOrModified(TEntity entity, int id)
        {
            DbEntityEntry dbEntityEntry = GetDbEntityEntrySafely(entity);

            if (id < 0)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                dbEntityEntry.State = EntityState.Modified;
            }
        }


        // Privates
        private DbEntityEntry GetDbEntityEntrySafely(TEntity entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            return dbEntityEntry;
        }

        private IQueryable<TEntity> Filter<TProperty>(IQueryable<TEntity> dbSet,
            Expression<Func<TEntity, TProperty>> property, TProperty value)
            where TProperty : IComparable
        {

            var memberExpression = property.Body as MemberExpression;
            if (memberExpression == null || !(memberExpression.Member is PropertyInfo))
            {

                throw new ArgumentException("Property expected", "property");
            }

            Expression left = property.Body;
            Expression right = Expression.Constant(value, typeof(TProperty));
            Expression searchExpression = Expression.Equal(left, right);
            Expression<Func<TEntity, bool>> lambda = Expression.Lambda<Func<TEntity, bool>>(
                searchExpression, new ParameterExpression[] { property.Parameters.Single() });

            return dbSet.Where(lambda);
        }

        private enum OrderByType
        {

            Ascending,
            Descending
        }
    }
}
