
using RestaurantApi.Dal.Models;

namespace RestaurantApi.Business.Services
{
    public interface ICRUDService<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllItemsAsync(string include = "");
        Task<T> GetItemByIdAsync(int id, string include = "");
        Task CreateItemAsync(T entity);
        Task UpdateItemAsync(T entity, int id);
        Task DeleteItemAsync(int id);
    }
}
