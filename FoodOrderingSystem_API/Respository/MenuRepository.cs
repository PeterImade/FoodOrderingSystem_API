using FoodOrderingSystem_API.Contracts;
using FoodOrderingSystem_API.Data;
using FoodOrderingSystem_API.Models;

namespace FoodOrderingSystem_API.Respository
{
    public class MenuRepository : Repository<Menu>, IMenuRepository
    {
        public MenuRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
