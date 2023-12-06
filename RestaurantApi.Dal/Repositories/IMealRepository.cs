
using RestaurantApi.Dal.Models;

namespace RestaurantApi.Dal.Repositories
{
    public interface IMealRepository : IRepository<Meal>
    {
        Task<IEnumerable<Meal>> GetInIdRangeAsync(int startId, int endId);
    }
}
