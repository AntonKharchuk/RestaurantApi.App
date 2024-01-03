using Microsoft.AspNetCore.Mvc;

using RestaurantApi.App.Models;
using RestaurantApi.Business.Services;

namespace RestaurantApi.App.Controllers
{
    [Route("/api/Order/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private IOrderItemCRUDService _orederItemsServise;

        public OrderItemsController(IOrderItemCRUDService orederItemsServise)
        {
            _orederItemsServise = orederItemsServise;
        }

        [HttpGet]
        public async Task<IEnumerable<OrderItem>> GetAllOrderItems([FromQuery] string orderId = "", [FromQuery] string includePath = "")
        {
            int odrerIdValue = 0;
            Task<IEnumerable<Dal.Models.OrderItem>> asyncResult;
            if (!string.IsNullOrEmpty(orderId) && int.TryParse(orderId, out odrerIdValue))
            {
                asyncResult = _orederItemsServise.GetItemByOrderIdAsync(odrerIdValue);
            }
            else
            {
                asyncResult = _orederItemsServise.GetAllItemsAsync(includePath);
            }

            var result = new List<OrderItem>();

            foreach (var item in await asyncResult)
            {
                result.Add(Parser.OrderItemFromDALToApp(item));
            }

            return result;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItem>> GetOrderItemById(int id, [FromQuery] string includePath = "")
        {
            var orderItem = await _orederItemsServise.GetItemByIdAsync(id, includePath);
            if (orderItem is null)
                return NotFound();

            return Parser.OrderItemFromDALToApp(orderItem);
        }

        [HttpPost]
        public async Task<ActionResult<OrderItem>> CreateOrderItem(OrderItem orderItem)
        {
            await _orederItemsServise.CreateItemAsync(Parser.OrderItemFromAppToDAL(orderItem));

            return Ok(orderItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderItem(int id, OrderItem orderItem)
        {
            await   _orederItemsServise.UpdateItemAsync(Parser.OrderItemFromAppToDAL(orderItem), id);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            await _orederItemsServise.DeleteItemAsync(id);

            return Ok();
        }

    }
}
