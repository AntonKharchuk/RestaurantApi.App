
using RestaurantApi.Dal.Models;

namespace RestaurantApi.Dal.Services
{
    public interface ICRUDService<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllItemsAsync();
        Task<T> GetItemByIdAsync(int id);
        Task CreateItemAsync(T entity);
        Task UpdateItemAsync(T entity, int id);
        Task DeleteItemAsync(int id);
    }
}
