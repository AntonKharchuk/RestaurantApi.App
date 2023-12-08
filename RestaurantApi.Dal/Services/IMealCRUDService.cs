
using RestaurantApi.Dal.Models;

namespace RestaurantApi.Dal.Services
{
    public interface IMealCRUDService: ICRUDService<Meal>
    {
        Task<IEnumerable<Meal>> GetItemsInRange(int startId, int endId, string include="");
    }
}
