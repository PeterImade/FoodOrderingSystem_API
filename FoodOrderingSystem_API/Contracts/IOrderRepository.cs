using FoodOrderingSystem_API.Models;
using System.Linq.Expressions;

namespace FoodOrderingSystem_API.Contracts
{
    public interface IOrderRepository: IRepository<Order>
    {
        Task<Order> GetAnOrder(Expression<Func<Order, bool>> expression = null, bool tracked = true);
    }
}
