using Microsoft.AspNetCore.Mvc;

using RestaurantApi.App.Models;
using RestaurantApi.Business.Services;

namespace RestaurantApi.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IOrdersService _ordersService;
        public OrderController(IOrdersService mealsServise)
        {
            _ordersService = mealsServise;
        }

        [HttpGet("Orders", Name = "GetAllOrders")]
        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            var asyncResult = _ordersService.OrderService.GetAllItemsAsync();

            var result = new List<Order>();

            foreach (var item in await asyncResult)
            {
                result.Add(Parser.OrderFromDALToApp(item));
            }

            return result;
        }

        [HttpGet("Orders/{id}", Name = "GetOrderById")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var order = await _ordersService.OrderService.GetItemByIdAsync(id);
            if (order is null)
                return NotFound();

            return Parser.OrderFromDALToApp(order);
        }

        [HttpPost("Orders", Name = "CreateOrder")]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            await _ordersService.OrderService.CreateItemAsync(Parser.OrderFromAppToDAL(order));

            return CreatedAtRoute("CreateOrder", new { id = order.Id }, order);
        }

        [HttpPut("Orders/{id}", Name = "UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(int id, Order order)
        {
            await _ordersService.OrderService.UpdateItemAsync(Parser.OrderFromAppToDAL(order), id);

            return Ok();
        }

        [HttpDelete("Orders/{id}", Name = "DeleteOrder")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _ordersService.OrderService.DeleteItemAsync(id);

            return Ok();
        }
        [HttpGet("OrderItems", Name = "GetAllOrderItems")]
        public async Task<IEnumerable<OrderItem>> GetAllOrderItems([FromQuery] string orderId = "",[FromQuery] string includePath = "")
        {
            int odrerIdValue=0;
            Task<IEnumerable<Dal.Models.OrderItem>> asyncResult;
            if (!string.IsNullOrEmpty(orderId)&&int.TryParse(orderId,out odrerIdValue))
            {
                asyncResult = _ordersService.OrderItemService.GetItemByOrderIdAsync(odrerIdValue);
            }
            else
            {
                asyncResult = _ordersService.OrderItemService.GetAllItemsAsync(includePath);
            }

            var result = new List<OrderItem>();

            foreach (var item in await asyncResult)
            {
                result.Add(Parser.OrderItemFromDALToApp(item));
            }

            return result;
        }
        [HttpGet("OrderItems/{id}", Name = "GetOrderItemById")]
        public async Task<ActionResult<OrderItem>> GetOrderItemById(int id, [FromQuery] string includePath = "")
        {
            var orderItem = await _ordersService.OrderItemService.GetItemByIdAsync(id, includePath);
            if (orderItem is null)
                return NotFound();

            return Parser.OrderItemFromDALToApp(orderItem);
        }

        [HttpPost("OrderItems", Name = "CreateOrderItem")]
        public async Task<ActionResult<OrderItem>> CreateOrderItem(OrderItem orderItem)
        {
            await _ordersService.OrderItemService.CreateItemAsync(Parser.OrderItemFromAppToDAL(orderItem));

            return CreatedAtRoute("CreateOrderItem", new { id = orderItem.Id }, orderItem);
        }

        [HttpPut("OrderItems/{id}", Name = "UpdateOrderItem")]
        public async Task<IActionResult> UpdateOrderItem(int id, OrderItem orderItem)
        {
            await _ordersService.OrderItemService.UpdateItemAsync(Parser.OrderItemFromAppToDAL(orderItem), id);

            return Ok();
        }

        [HttpDelete("OrderItems/{id}", Name = "DeleteOrderItem")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            await _ordersService.OrderItemService.DeleteItemAsync(id);

            return Ok();
        }

    }
}
