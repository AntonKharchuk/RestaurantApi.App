
using RestaurantApi.Dal.Models;
using RestaurantApi.Dal.Repositories;

namespace RestaurantApi.Business.Services
{
    public class MealCRUDService : CRUDService<Meal>, IMealCRUDService
    {
        private readonly IMealRepository _repository;

        public MealCRUDService(IMealRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Meal>> GetItemsInRangeAsync(int startId, int endId, string include = "")
        {
            return await _repository.GetInIdRangeAsync(startId, endId, include);
        }
    }
}
