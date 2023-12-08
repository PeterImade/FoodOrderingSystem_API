using FoodOrderingSystem_API.Models;
using System.Linq.Expressions;

namespace FoodOrderingSystem_API.Contracts
{
    public interface IRestaurantRepository: IRepository<Restaurant>
    {
        Task<Restaurant> GetRestaurant(Expression<Func<Restaurant, bool>> expression = null, bool tracked = true);
    }
}
