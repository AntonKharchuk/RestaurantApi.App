using Microsoft.AspNetCore.Mvc;

using RestaurantApi.App.Models;
using RestaurantApi.Business.Services;

namespace RestaurantApi.App.Controllers
{
    [Route("api/Meal/[controller]")]
    [ApiController]
    public class PriceListItemsController : ControllerBase
    {
        private ICRUDService<Dal.Models.PriceListItem> _priceListItemServiсe;

        public PriceListItemsController(ICRUDService<Dal.Models.PriceListItem> priceListItemServise)
        {
            _priceListItemServiсe = priceListItemServise;
        }


        [HttpGet]
        public async Task<IEnumerable<PriceListItem>> GetAllPriceListItems([FromQuery] string includePath = "")
        {
            var asyncResult = _priceListItemServiсe.GetAllItemsAsync(includePath);

            var result = new List<PriceListItem>();

            foreach (var item in await asyncResult)
            {
                result.Add(Parser.PriceListItemFromDALToApp(item));
            }

            return result;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<PriceListItem>> GetPriceListItemById(int id, [FromQuery] string includePath = "")
        {
            var priceListItem = await _priceListItemServiсe.GetItemByIdAsync(id, includePath);
            if (priceListItem is null)
                return NotFound();

            return Parser.PriceListItemFromDALToApp(priceListItem);
        }
        [HttpPost]
        public async Task<ActionResult<PriceListItem>> CreatePriceListItem(PriceListItem priceListItem)
        {
            await _priceListItemServiсe.CreateItemAsync(Parser.PriceListItemFromAppToDAL(priceListItem));

            return Ok(priceListItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePriceListItem(int id, PriceListItem priceListItem)
        {
            await _priceListItemServiсe.UpdateItemAsync(Parser.PriceListItemFromAppToDAL(priceListItem), id);

            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePriceListItem(int id)
        {
            await _priceListItemServiсe.DeleteItemAsync(id);

            return Ok();
        }
    }
}
