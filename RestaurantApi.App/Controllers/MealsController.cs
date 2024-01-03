using Microsoft.AspNetCore.Mvc;

using RestaurantApi.App.Models;
using RestaurantApi.Business.Services;

namespace RestaurantApi.App.Controllers
{
    [Route("api/Meal/[controller]")]
    [ApiController]
    public class MealsController : ControllerBase
    {
        private IMealCRUDService _mealsService;

        public MealsController(IMealCRUDService mealsService)
        {
            _mealsService = mealsService;
        }

        [HttpGet]
        public async Task<IEnumerable<Meal>> GetAllMeals([FromQuery] string includePath = "")
        {
            var asyncResult = _mealsService.GetAllItemsAsync(includePath);

            var result = new List<Meal>();

            foreach (var item in await asyncResult)
            {
                result.Add(Parser.MealFromDALToApp(item));
            }

            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Meal>> GetMealById(int id, [FromQuery] string includePath = "")
        {
            var meal = await _mealsService.GetItemByIdAsync(id, includePath);
            if (meal is null)
                return NotFound();

            return Parser.MealFromDALToApp(meal);
        }
        [HttpGet("Page/{num}")]
        public async Task<IEnumerable<Meal>> GetMealByPage(int num, [FromQuery] string includePath = "")
        {
            var asyncResult = _mealsService.GetItemsInRangeAsync(10 * (num - 1), 10 * (num), includePath);

            var result = new List<Meal>();

            foreach (var item in asyncResult.Result)
            {
                result.Add(Parser.MealFromDALToApp(item));
            }

            return result;
        }

        [HttpPost]
        public async Task<ActionResult<Meal>> CreateMeal(Meal meal)
        {
            await _mealsService.CreateItemAsync(Parser.MealFromAppToDAL(meal));

            return Ok(meal);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMeal(int id, Meal meal)
        {
            await _mealsService.UpdateItemAsync(Parser.MealFromAppToDAL(meal), id);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeal(int id)
        {
            await _mealsService.DeleteItemAsync(id);

            return Ok();
        }
    }
}
