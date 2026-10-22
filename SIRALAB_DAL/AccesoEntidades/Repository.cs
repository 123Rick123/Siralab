using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Objects;
using System.Linq.Expressions;
using System.Data;
using System.Data.Objects.DataClasses;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;


namespace SIRALAB_DAL
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private DbContext _context;
        private DbSet<TEntity> _dbSet { get { return _context.Set<TEntity>(); } }
        //private ObjectContext _context;
        /*private readonly IObjectSet<TEntity> _objectSet;*/

        /*
        public Repository(ObjectContext context)
        {
            _context = context;
            _objectSet = _context.CreateObjectSet<TEntity>();
            _context.ContextOptions.LazyLoadingEnabled = true; //Default!
        }*/

        public Repository(DbContext context)
        {
            _context = context;
            /*_objectSet = _context.CreateObjectSet<TEntity>();*/
            var contextTest = ((IObjectContextAdapter)_context).ObjectContext;
            var entityTest = _context.ChangeTracker.Entries().Select(c => c.Entity).ToList();
           // contextTest.Refresh(RefreshMode.StoreWins, entityTest);

           // _context.Entry<Entity>(entityTest).Reload();

            _context.Configuration.LazyLoadingEnabled = true; //Default!
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            _dbSet.Add(entity);
            this.SaveChanges();
        }

        public int NextNewID(Expression<Func<TEntity, int>> predicate)
        {
            var id = !_dbSet.Any() ? 1 : _dbSet.Max(predicate) + 1;
            return id;
        }

        public void Edit(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            _dbSet.Remove(entity);
            _context.SaveChanges();
        }
        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var records = Find(predicate);

            foreach (var record in records)
            {
                Delete(record);
            }
        }

        public void DeleteRelatedEntities(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            var releatedEntities =
                ((IEntityWithRelationships)entity).RelationshipManager.GetAllRelatedEnds().SelectMany(
                    e => e.CreateSourceQuery().OfType<TEntity>());
            foreach (var releatedEntity in releatedEntities)
            {
                _dbSet.Remove(releatedEntity);
            }
            _dbSet.Remove(entity);

        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet.AsEnumerable();//.ToList();
        }
        public IEnumerable<TEntity> GetAllPaged(int page, int pageSize)
        {
            return _dbSet.Skip(pageSize).Take(page);
        }

        public IQueryable<TEntity> FindQueryable(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate).AsEnumerable();
        }
        public IEnumerable<TEntity> FindPaged(int page, int pageSize, Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, DateTime>> OrderByDate)
        {
            IEnumerable<TEntity> result = _dbSet.Where(predicate).OrderBy<TEntity, DateTime>(OrderByDate).Skip(pageSize * page).Take(pageSize).AsEnumerable();

            return result;
        }
        public IEnumerable<TEntity> FindPaged(int page, int pageSize, Expression<Func<TEntity, bool>> predicate, List<Func<TEntity, object>> lstOrderBy, string sortDirection)
        {
            IEnumerable<TEntity> result = (sortDirection == "ASC") ? _dbSet.Where(predicate).OrderBy(lstOrderBy[0]).Skip(pageSize * page).Take(pageSize).AsEnumerable() : _dbSet.Where(predicate).OrderByDescending(lstOrderBy[0]).Skip(pageSize * page).Take(pageSize).AsEnumerable();

            return result;
        }
        public IEnumerable<TEntity> FindPaged(int page, int pageSize, Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate).Skip(pageSize * page).Take(pageSize).AsEnumerable();
        }


        public TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.SingleOrDefault(predicate);
        }
        public TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public int Count()
        {
            return _dbSet.Count();
        }

        public int Count(Expression<Func<TEntity, bool>> criteria)
        {
            return _dbSet.Where(criteria).Count();
        }

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (_context == null) return;
            _context.Dispose();
            _context = null;
        } 
        #endregion

        /*~Repository()
        {
            this.Dispose();
        }*/
    }
}
