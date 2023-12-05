using FoodOrderingSystem_API.DTOs.OrderItem;
using FoodOrderingSystem_API.Models;

namespace FoodOrderingSystem_API.DTOs.Customer
{
    public abstract class BaseDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNo { get; set; }
        
    }
}
