
using RestaurantApi.Dal.Models;

namespace RestaurantApi.Dal.Services
{
    public interface ICRUDService<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllItems();
        Task<T> GetItemById(int id);
        Task CreateItem(T entity);
        Task UpdateItem(T entity, int id);
        Task DeleteItem(int id);
    }
}
