using FoodOrderingSystem_API.Contracts;
using FoodOrderingSystem_API.Data;
using FoodOrderingSystem_API.Models;

namespace FoodOrderingSystem_API.Respository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
