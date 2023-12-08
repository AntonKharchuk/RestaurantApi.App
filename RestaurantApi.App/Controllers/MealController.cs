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
        public IEnumerable<Ingredient> GetAllIngredients()
        {
            var asyncResult = _mealsService.IngredientService.GetAllItemsAsync();

            var result = new List<Ingredient>();

            foreach (var item in asyncResult.Result)
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

    }
}
