
using RestaurantApi.Dal.Models;

namespace RestaurantApi.Dal.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T>? GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity, int id);
        Task DeleteAsync(int id);
        Task SaveAsync();

    }
}
