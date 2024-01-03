using Microsoft.AspNetCore.Mvc;

using RestaurantApi.App.Models;
using RestaurantApi.Business.Services;

namespace RestaurantApi.App.Controllers
{
    [Route("api/Meals/[controller]")]
    [ApiController]
    public class PortionsController : ControllerBase
    {
        private ICRUDService<Dal.Models.Portion> _portionServise;

        public PortionsController(ICRUDService<Dal.Models.Portion> portionServise)
        {
            _portionServise = portionServise;
        }
        [HttpGet]
        public async Task<IEnumerable<Portion>> GetAllPortions()
        {
            var asyncResult = _portionServise.GetAllItemsAsync();

            var result = new List<Portion>();

            foreach (var item in await asyncResult)
            {
                result.Add(Parser.PortionFromDALToApp(item));
            }

            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Portion>> GetPortionById(int id)
        {
            var portion = await _portionServise.GetItemByIdAsync(id);
            if (portion is null)
                return NotFound();

            return Parser.PortionFromDALToApp(portion);
        }

        [HttpPost]
        public async Task<ActionResult<Portion>> CreatePortion(Portion portion)
        {
            await _portionServise.CreateItemAsync(Parser.PortionFromAppToDAL(portion));

            return Ok(portion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePortion(int id, [FromBody] Portion portion)
        {
            await _portionServise.UpdateItemAsync(Parser.PortionFromAppToDAL(portion), id);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePortion(int id)
        {
            await _portionServise.DeleteItemAsync(id);

            return Ok();
        }
    }
}
