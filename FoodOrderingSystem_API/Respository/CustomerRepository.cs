using FoodOrderingSystem_API.Contracts;
using FoodOrderingSystem_API.Data;
using FoodOrderingSystem_API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FoodOrderingSystem_API.Respository
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly DbSet<Customer> dbSet;

        public CustomerRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.Set<Customer>();
        }

        public async Task<Customer> GetCustomer(Expression<Func<Customer, bool>> expression = null, bool tracked = true)
        {
            IQueryable<Customer> query = dbSet;

            if (!tracked)
            {
                query = query.AsNoTracking();   
            }

            if (expression != null)
            {
                query = query.Where(expression);
            }

            query = query.Include(order => order.Orders);

            return await query.FirstOrDefaultAsync();
        }
    }
}
