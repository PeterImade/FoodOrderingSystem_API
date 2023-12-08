using FoodOrderingSystem_API.Contracts;
using FoodOrderingSystem_API.Data;
using FoodOrderingSystem_API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FoodOrderingSystem_API.Respository
{
    public class RestaurantRepository: Repository<Restaurant>, IRestaurantRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly DbSet<Restaurant> _dbSet;
        public RestaurantRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
            _dbSet = dbContext.Set<Restaurant>();
        }

        public async Task<Restaurant> GetRestaurant(Expression<Func<Restaurant, bool>> expression = null, bool tracked = true)
        {
            IQueryable<Restaurant> query = _dbSet;

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            if (expression != null)
            {
                query = query.Where(expression);
            }

            query = query.Include(menu => menu.Menus);

            return await query.FirstOrDefaultAsync();
        }
    }
}
