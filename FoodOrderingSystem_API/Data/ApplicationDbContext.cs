using FoodOrderingSystem_API.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingSystem_API.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            
        }
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>()
                .HavePrecision(18, 6);
        }
        
        public DbSet<Restaurant> Restaurants  { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Customer> Customers { get; set; }
        
        public DbSet<Order> Orders  { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        
    }
}
