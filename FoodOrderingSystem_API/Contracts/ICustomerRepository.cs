
using FoodOrderingSystem_API.Models;
using System.Linq.Expressions;

namespace FoodOrderingSystem_API.Contracts
{
    public interface ICustomerRepository: IRepository<Customer>
    {
        Task<Customer> GetCustomer(Expression<Func<Customer, bool>> expression = null, bool tracked = true);
    }
}
