using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderingSystem_API.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }

        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
