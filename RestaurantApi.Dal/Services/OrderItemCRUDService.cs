
using RestaurantApi.Dal.Models;
using RestaurantApi.Dal.Repositories;

namespace RestaurantApi.Dal.Services
{
    public class OrderItemCRUDService : CRUDService<OrderItem>, IOrderItemCRUDService
    {
        private readonly IOrderItemRepository _repository;

        public OrderItemCRUDService(IOrderItemRepository repository) : base(repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<OrderItem>> GetItemByOrderIdAsync(int id, string includes = "")
        {
            return await _repository.GetByOrderId(id, includes);
        }
    }
}
