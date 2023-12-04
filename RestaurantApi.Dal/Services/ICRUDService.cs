
namespace RestaurantApi.Dal.Services
{
    public interface ICRUDService<T> where T : class
    {
        Task<IEnumerable<T>> GetAllItems();
        Task<T> GetItemById(int id);
        Task CreateItem(T entity);
        Task UpdateItem(T entity, int id);
        Task DeleteItem(int id);
    }
}
