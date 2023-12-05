using FoodOrderingSystem_API.DTOs.OrderItem;
using FoodOrderingSystem_API.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderingSystem_API.DTOs.Menu
{
    public abstract class BaseDTO
    {
        public string Name { get; set; }
        public string Description { get; set; } 
        public decimal Price { get; set; } 
        public int restaurantId { get; set; }
    }
}

