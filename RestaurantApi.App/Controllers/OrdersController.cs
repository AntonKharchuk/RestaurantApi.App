using Microsoft.AspNetCore.Mvc;

using RestaurantApi.App.Models;
using RestaurantApi.Business.Services;

namespace RestaurantApi.App.Controllers
{
    [Route("/api/Order/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private ICRUDService<Dal.Models.Order> _orserServise;

        public OrdersController(ICRUDService<Dal.Models.Order> orserServise)
        {
            _orserServise = orserServise;
        }

        [HttpGet]
        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            var asyncResult = _orserServise.GetAllItemsAsync();

            var result = new List<Order>();

            foreach (var item in await asyncResult)
            {
                result.Add(Parser.OrderFromDALToApp(item));
            }

            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var order = await _orserServise.GetItemByIdAsync(id);
            if (order is null)
                return NotFound();

            return Parser.OrderFromDALToApp(order);
        }
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            await _orserServise.CreateItemAsync(Parser.OrderFromAppToDAL(order));

            return Ok(order);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateOrder(int id, Order order)
        {
            await _orserServise.UpdateItemAsync(Parser.OrderFromAppToDAL(order), id);

            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orserServise.DeleteItemAsync(id);

            return Ok();
        }
    }
}
