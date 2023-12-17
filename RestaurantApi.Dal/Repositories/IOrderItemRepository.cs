
using RestaurantApi.Dal.Models;

namespace RestaurantApi.Dal.Repositories
{
    public interface IOrderItemRepository:IRepository<OrderItem>
    {
        Task<IEnumerable<OrderItem>> GetByOrderId(int id, string includes = "");
    }
}
