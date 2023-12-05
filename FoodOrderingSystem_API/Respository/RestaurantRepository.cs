using FoodOrderingSystem_API.Contracts;
using FoodOrderingSystem_API.Data;
using FoodOrderingSystem_API.Models;

namespace FoodOrderingSystem_API.Respository
{
    public class RestaurantRepository: Repository<Restaurant>, IRestaurantRepository
    {
        public RestaurantRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
