using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq;
using System.Linq.Expressions;

namespace notification_system.notification.Persistence.Base
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        internal readonly DbContext _context;
        internal readonly DbSet<T> _dbSet;

        public RepositoryBase(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Add(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public async Task AddAsync(T entity, CancellationToken cs = default)
        {
            await _dbSet.AddAsync(entity, cs);
        }

        public async Task AddAsync(IEnumerable<T> entities, CancellationToken cs = default)
        {
            await _dbSet.AddRangeAsync(entities, cs);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
            _context.Entry(entity).State = EntityState.Deleted;
        }

        public void Delete(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            _context.Entry(entities).State = EntityState.Deleted;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression);
        }

        public IQueryable<T> Query(Expression<Func<T, bool>>? expression = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, Func<IQueryable<T>, IOrderedQueryable<T>>? order = null)
        {
            IQueryable<T> query = _dbSet;

            if (include is not null)
            {
                query = include(query);
            }
            if (expression is not null)
            {
                query = query.Where(expression);
            }
            if (order is not null)
            {
                query = order(query);
            }

            return query;
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Update(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
            _context.Entry(entities).State = EntityState.Modified;
        }
    }
}
