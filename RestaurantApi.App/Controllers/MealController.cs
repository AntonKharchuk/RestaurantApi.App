using Microsoft.AspNetCore.Mvc;

using RestaurantApi.App.Models;
using RestaurantApi.Dal.Services;

namespace RestaurantApi.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealController : ControllerBase
    {
        private IMealsService _mealsService;
        public MealController(IMealsService mealsServise)
        {
            _mealsService = mealsServise;
        }

        [HttpGet("Ingredients", Name = "GetAllIngredients")]
        public async Task<IEnumerable<Ingredient>> GetAllIngredients()
        {
            var asyncResult = _mealsService.IngredientService.GetAllItemsAsync();

            var result = new List<Ingredient>();

            foreach (var item in await asyncResult)
            {
                result.Add(Parser.IngredientFromDALToApp(item));
            }

            return result;
        }


        [HttpGet("Ingredients/{id}", Name = "GetIngredientById")]
        public async Task<ActionResult<Ingredient>> GetIngredientById(int id)
        {
            var ingredient = await _mealsService.IngredientService.GetItemByIdAsync(id);
            if (ingredient is null)
                return NotFound();

            return Parser.IngredientFromDALToApp(ingredient);
        }

        [HttpPost("Ingredients", Name = "CreateIngredient")]
        public async Task<ActionResult<Ingredient>> CreateIngredient(Ingredient ingredient)
        {
            await _mealsService.IngredientService.CreateItemAsync(Parser.IngredientFromAppToDAL(ingredient));

            return CreatedAtRoute("CreateIngredient", new { id = ingredient.Id }, ingredient);
        }

        [HttpPut("Ingredients", Name = "UpdateIngredient")]
        public async Task<IActionResult> UpdateIngredient([FromQuery] int id, [FromBody] Ingredient ingredient)
        {
            await _mealsService.IngredientService.UpdateItemAsync(Parser.IngredientFromAppToDAL(ingredient), id);

            return Ok();
        }

        [HttpDelete("Ingredients/{id}", Name = "DeleteIngredient")]
        public async Task<IActionResult> DeleteIngredient(int id)
        {
            await _mealsService.IngredientService.DeleteItemAsync(id);

            return Ok();
        }


        [HttpGet("Meals", Name = "GetAllMeals")]
        public async Task<IEnumerable<Meal>> GetAllMeals([FromQuery] string includePath = "")
        {
            var asyncResult = _mealsService.MealService.GetAllItemsAsync(includePath);

            var result = new List<Meal>();

            foreach (var item in await asyncResult)
            {
                result.Add(Parser.MealFromDALToApp(item));
            }

            return result;
        }

        [HttpGet("Meals/{id}", Name = "GetMealById")]
        public async Task<ActionResult<Meal>> GetMealById(int id, [FromQuery] string includePath = "")
        {
            var meal = await _mealsService.MealService.GetItemByIdAsync(id, includePath);
            if (meal is null)
                return NotFound();

            return Parser.MealFromDALToApp(meal);
        }
        [HttpGet("Meals/Page/{num}", Name = "GetMealByPage")]
        public async Task<IEnumerable<Meal>> GetMealByPage(int num, [FromQuery] string includePath = "")
        {
            var asyncResult = _mealsService.MealService.GetItemsInRange(10*(num-1), 10*(num), includePath);

            var result = new List<Meal>();

            foreach (var item in asyncResult.Result)
            {
                result.Add(Parser.MealFromDALToApp(item));
            }

            return result;
        }

        [HttpPost("Meals", Name = "CreateMeal")]
        public async Task<ActionResult<Meal>> CreateMeal(Meal meal)
        {
            await _mealsService.MealService.CreateItemAsync(Parser.MealFromAppToDAL(meal));

            return CreatedAtRoute("CreateMeal", new { id = meal.Id }, meal);
        }

        [HttpPut("Meals/{id}", Name = "UpdateMeal")]
        public async Task<IActionResult> UpdateMeal(int id, Meal meal)
        {
            await _mealsService.MealService.UpdateItemAsync(Parser.MealFromAppToDAL(meal), id);

            return Ok();
        }

        [HttpDelete("Meals/{id}", Name = "DeleteMeal")]
        public async Task<IActionResult> DeleteMeal(int id)
        {
            await _mealsService.MealService.DeleteItemAsync(id);

            return Ok();
        }

    }
}
