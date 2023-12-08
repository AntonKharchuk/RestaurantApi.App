
using RestaurantApi.Dal.Models;
using RestaurantApi.Dal.Repositories;

namespace RestaurantApi.Dal.Services
{
    public class CRUDService<T> : ICRUDService<T> where T : BaseEntity
    {
        private readonly IRepository<T> _repository;

        public CRUDService(IRepository<T> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<T>> GetAllItemsAsync(string include = "")
        {
            return await _repository.GetAllAsync(include);
        }

        public async Task<T> GetItemByIdAsync(int id, string include = "")
        {
            return await _repository.GetByIdAsync(id, include);
        }

        public async Task CreateItemAsync(T entity)
        {
            await _repository.AddAsync(entity);
            await _repository.SaveAsync();
        }

        public async Task UpdateItemAsync(T entity, int id)
        {
            await _repository.UpdateAsync(entity, id);
            await _repository.SaveAsync();
        }

        public async Task DeleteItemAsync(int id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveAsync();
        }
    }
}
