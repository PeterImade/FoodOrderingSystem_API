using FoodOrderingSystem_API.DTOs.Order;

namespace FoodOrderingSystem_API.DTOs.Customer
{
    public class CustomerDTO: BaseDTO
    {
        public int Id { get; set; }
        public ICollection<OrderDTO> Orders { get; set; }
    }
}
