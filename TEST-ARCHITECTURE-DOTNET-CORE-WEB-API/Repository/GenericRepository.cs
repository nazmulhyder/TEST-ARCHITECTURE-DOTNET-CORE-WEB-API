using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.Data;
using TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.IRepository;

namespace TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<T> _db;

        public GenericRepository(DatabaseContext context)
        {
            this._context = context;
            this._db = this._context.Set<T>();
        }
        public async Task<IList<T>> GetAll(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<string> includes = null)
        {
            IQueryable<T> query = this._db;

            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (includes != null)
            {
                foreach (var includeProperty in includes)
                {
                    query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression, List<string> includes = null)
        {
            IQueryable<T> query = this._db;
            if (includes != null)
            {
                foreach (var includeProperty in includes)
                {
                    query.Include(includeProperty);
                }
            }

            return await query.AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task Insert(T entity)
        {
            await this._db.AddAsync(entity);
        }

        public async Task InsertRange(IEnumerable<T> entities)
        {
            await this._db.AddRangeAsync(entities);
        }

        public async Task Delete(int id)
        {
            var entity = await this._db.FindAsync(id);
            this._db.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            this._db.RemoveRange(entities);
        }

        public void Update(T entity)
        {
            this._db.Attach(entity);
            this._context.Entry(entity).State = EntityState.Modified;
        }
    }
}
