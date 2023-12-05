using FoodOrderingSystem_API.DTOs.OrderItem;

namespace FoodOrderingSystem_API.DTOs.Menu
{
    public class MenuDTO: BaseDTO
    {
        public int Id { get; set; }
        public ICollection<OrderItemDTO> OrderItems { get; set; }
    }
}
