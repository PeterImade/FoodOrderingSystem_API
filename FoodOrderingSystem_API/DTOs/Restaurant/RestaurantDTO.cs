using FoodOrderingSystem_API.DTOs.Menu;

namespace FoodOrderingSystem_API.DTOs.RestaurantDTO
{
    public class RestaurantDTO: BaseDTO
    {
        public int Id { get; set; }
        public ICollection<MenuDTO> Menus { get; set; }
    }
}
