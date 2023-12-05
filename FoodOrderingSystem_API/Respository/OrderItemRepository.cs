using FoodOrderingSystem_API.Contracts;
using FoodOrderingSystem_API.Data;
using FoodOrderingSystem_API.Models;

namespace FoodOrderingSystem_API.Respository
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
