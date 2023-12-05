namespace FoodOrderingSystem_API.DTOs.OrderItem
{
    public abstract class BaseDTO
    {
        public int OrderId { get; set; }
        public int MenuId { get; set; }
    }
}
