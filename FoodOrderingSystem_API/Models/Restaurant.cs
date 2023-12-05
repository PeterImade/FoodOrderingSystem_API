using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderingSystem_API.Models
{
    public class Restaurant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } 
        public string Address { get; set; }
        public string Website { get; set; }
        public long PhoneNo { get; set; }
        public ICollection<Menu> Menus { get; set; }
    }
}
