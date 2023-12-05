using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderingSystem_API.Models
{
    public class Menu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Precision(18,4)]
        public decimal Price { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }

        [ForeignKey(nameof(Restaurant))]
        public int restaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
