
using RestaurantApi.Dal.Models;

namespace RestaurantApi.Dal.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task Add(T entity);
        Task Update(T entity, int id);
        Task Delete(int id);
        Task Save();

    }
}
