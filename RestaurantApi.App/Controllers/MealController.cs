using Microsoft.AspNetCore.Mvc;

using RestaurantApi.App.Models;
using RestaurantApi.Business.Services;

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
            var asyncResult = _mealsService.MealService.GetItemsInRangeAsync(10*(num-1), 10*(num), includePath);

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

        [HttpGet("Portions", Name = "GetAllPortions")]
        public async Task<IEnumerable<Portion>> GetAllPortions()
        {
            var asyncResult = _mealsService.PortionService.GetAllItemsAsync();

            var result = new List<Portion>();

            foreach (var item in await asyncResult)
            {
                result.Add(Parser.PortionFromDALToApp(item));
            }

            return result;
        }

        [HttpGet("Portions/{id}", Name = "GetPortionById")]
        public async Task<ActionResult<Portion>> GetPortionById(int id)
        {
            var portion = await _mealsService.PortionService.GetItemByIdAsync(id);
            if (portion is null)
                return NotFound();

            return Parser.PortionFromDALToApp(portion);
        }

        [HttpPost("Portions", Name = "CreatePortion")]
        public async Task<ActionResult<Portion>> CreatePortion(Portion portion)
        {
            await _mealsService.PortionService.CreateItemAsync(Parser.PortionFromAppToDAL(portion));

            return CreatedAtRoute("CreatePortion", new { id = portion.Id }, portion);
        }

        [HttpPut("Portions", Name = "UpdatePortion")]
        public async Task<IActionResult> UpdatePortion([FromQuery] int id, [FromBody] Portion portion)
        {
            await _mealsService.PortionService.UpdateItemAsync(Parser.PortionFromAppToDAL(portion), id);

            return Ok();
        }

        [HttpDelete("Portions/{id}", Name = "DeletePortion")]
        public async Task<IActionResult> DeletePortion(int id)
        {
            await _mealsService.PortionService.DeleteItemAsync(id);

            return Ok();
        }
        [HttpGet("PriceListItems", Name = "GetAllPriceListItems")]
        public async Task<IEnumerable<PriceListItem>> GetAllPriceListItems([FromQuery] string includePath = "")
        {
            var asyncResult = _mealsService.PriceListService.GetAllItemsAsync(includePath);
            
            var result = new List<PriceListItem>();

            foreach (var item in await asyncResult)
            {
                result.Add(Parser.PriceListItemFromDALToApp(item));
            }

            return result;
        }

        [HttpGet("PriceListItems/{id}", Name = "GetPriceListItemById")]
        public async Task<ActionResult<PriceListItem>> GetPriceListItemById(int id, [FromQuery] string includePath = "")
        {
            var priceListItem = await _mealsService.PriceListService.GetItemByIdAsync(id, includePath);
            if (priceListItem is null)
                return NotFound();

            return Parser.PriceListItemFromDALToApp(priceListItem);
        }

        [HttpPost("PriceListItems", Name = "CreatePriceListItem")]
        public async Task<ActionResult<PriceListItem>> CreatePriceListItem(PriceListItem priceListItem)
        {
            await _mealsService.PriceListService.CreateItemAsync(Parser.PriceListItemFromAppToDAL(priceListItem));

            return CreatedAtRoute("CreatePriceListItem", new { id = priceListItem.Id }, priceListItem);
        }

        [HttpPut("PriceListItems/{id}", Name = "UpdatePriceListItem")]
        public async Task<IActionResult> UpdatePriceListItem(int id, PriceListItem priceListItem)
        {
            await _mealsService.PriceListService.UpdateItemAsync(Parser.PriceListItemFromAppToDAL(priceListItem), id);

            return Ok();
        }

        [HttpDelete("PriceListItems/{id}", Name = "DeletePriceListItem")]
        public async Task<IActionResult> DeletePriceListItem(int id)
        {
            await _mealsService.PriceListService.DeleteItemAsync(id);

            return Ok();
        }
    }
}
