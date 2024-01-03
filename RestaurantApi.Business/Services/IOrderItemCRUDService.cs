
using RestaurantApi.Dal.Models;

namespace RestaurantApi.Business.Services
{
    public interface IOrderItemCRUDService: ICRUDService<OrderItem>
    {
        Task<IEnumerable<OrderItem>> GetItemByOrderIdAsync(int id, string includes = "");
    }
}
