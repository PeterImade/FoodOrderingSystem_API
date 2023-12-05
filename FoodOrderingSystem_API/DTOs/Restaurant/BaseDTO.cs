using FoodOrderingSystem_API.DTOs.Menu;

namespace FoodOrderingSystem_API.DTOs.RestaurantDTO
{
    public abstract class BaseDTO
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
        public long PhoneNo { get; set; }
  
    }
}
