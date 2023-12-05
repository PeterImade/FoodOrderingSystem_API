using FoodOrderingSystem_API.DTOs.OrderItem;

namespace FoodOrderingSystem_API.DTOs.Order
{
    public class OrderDTO: BaseDTO
    {
        public int Id { get; set; }
        public ICollection<OrderItemDTO> OrderItems { get; set; }
    }
}
