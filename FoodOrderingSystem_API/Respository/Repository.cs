using FoodOrderingSystem_API.Contracts;
using FoodOrderingSystem_API.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace FoodOrderingSystem_API.Respository
{
    public class Repository<T>: IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext dbContext)
        {
            this._db = dbContext;
            this._dbSet = _db.Set<T>();
        }
        public async Task CreateAsync(T entity)
        { 
            await _dbSet.AddAsync(entity);
            await SaveAsync(); 
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression = null)
        {             
            IQueryable<T> query = _dbSet;

            if (expression != null)
            { 
                query = query.Where(expression);
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> expression = null, bool tracked = true)
        {
            IQueryable<T> query = _dbSet;

            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (expression != null)
            {
                query =  query.Where(expression);
            }

            return await query.FirstOrDefaultAsync();
        } 

        public async Task RemoveAsync(T entity)
        {
             _dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
           await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await SaveAsync(); 
        } 
    }
}
