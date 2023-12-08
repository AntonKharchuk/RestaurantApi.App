
using RestaurantApi.Dal.Models;

namespace RestaurantApi.Dal.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync(string include = "");
        Task<T>? GetByIdAsync(int id, string include = "");
        Task AddAsync(T entity);
        Task UpdateAsync(T entity, int id);
        Task DeleteAsync(int id);
        Task SaveAsync();

    }
}
