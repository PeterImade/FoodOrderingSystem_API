using System.Linq.Expressions;

namespace FoodOrderingSystem_API.Contracts
{
    public interface IRepository<T> where T : class
    {
        Task CreateAsync(T entity);
        Task RemoveAsync(T entity);
        Task UpdateAsync(T entity);
        Task<T> GetAsync(Expression<Func<T, bool>> expression = null, bool tracked = true);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression = null, bool tracked = true);
        Task SaveAsync();
    }
}
