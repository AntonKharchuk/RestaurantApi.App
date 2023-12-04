
using RestaurantApi.Dal.Repositories;

namespace RestaurantApi.Dal.Services
{
    public class CRUDService<T> : ICRUDService<T> where T : class
    {
        private readonly IRepository<T> _repository;

        public CRUDService(IRepository<T> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<T>> GetAllItems()
        {
            return await _repository.GetAll();
        }

        public async Task<T> GetItemById(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task CreateItem(T entity)
        {
            await _repository.Add(entity);
            await _repository.Save();
        }

        public async Task UpdateItem(T entity, int id)
        {
            await _repository.Update(entity, id);
            await _repository.Save();
        }

        public async Task DeleteItem(int id)
        {
            await _repository.Delete(id);
            await _repository.Save();
        }
    }
}
