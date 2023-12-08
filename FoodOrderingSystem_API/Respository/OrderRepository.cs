using FoodOrderingSystem_API.Contracts;
using FoodOrderingSystem_API.Data;
using FoodOrderingSystem_API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FoodOrderingSystem_API.Respository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly DbSet<Order> _dbSet;

        public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
            _dbSet = dbContext.Set<Order>();
        }

        public async Task<Order> GetAnOrder(Expression<Func<Order, bool>> expression = null, bool tracked = true)
        {
            IQueryable<Order> query = _dbSet;

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            if (expression != null)
            {
                query = query.Where(expression);
            }

            query = query.Include(order => order.OrderItems);
            
            return await query.FirstOrDefaultAsync();

        }
    }
}
