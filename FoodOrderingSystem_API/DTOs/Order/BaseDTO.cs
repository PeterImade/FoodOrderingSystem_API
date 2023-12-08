using FoodOrderingSystem_API.DTOs.OrderItem;
using FoodOrderingSystem_API.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderingSystem_API.DTOs.Order
{
    public abstract class BaseDTO
    {
        public DateTime Date { get; set; }
        public EOrderStatus Status { get; set; }
        public int CustomerId { get; set; }
    }
}
