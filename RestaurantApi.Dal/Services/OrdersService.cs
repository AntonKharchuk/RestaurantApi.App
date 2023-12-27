
using RestaurantApi.Dal.Models;

namespace RestaurantApi.Dal.Services
{
    public class OrdersService : IOrdersService
    {
        
        public ICRUDService<Order> OrderService { get; set; }
        public IOrderItemCRUDService OrderItemService { get; set; }

        public OrdersService(ICRUDService<Order> orderService,
                            IOrderItemCRUDService orderItemService
            )
        {
            OrderService = orderService;
            OrderItemService = orderItemService;
        }
    }
}
