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
            var asyncResult = _mealsService.IngredientService.GetAllItems();

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
            var ingredient = await _mealsService.IngredientService.GetItemById(id);

            if (ingredient == null)
            {
                return NotFound();
            }

            return Parser.IngredientFromDALToApp(ingredient);
        }

        [HttpPost("Ingredients", Name = "CreateIngredient")]
        public async Task<ActionResult<Ingredient>> CreateIngredient(Ingredient ingredient)
        {
            await _mealsService.IngredientService.CreateItem(Parser.IngredientFromAppToDAL(ingredient));

            return CreatedAtRoute("GetIngredientById", new { id = ingredient.Id }, ingredient);
        }

        [HttpPut("Ingredients", Name = "UpdateIngredient")]
        public async Task<IActionResult> UpdateIngredient([FromQuery] int id, [FromBody] Ingredient ingredient)
        {
            try
            {
                await _mealsService.IngredientService.UpdateItem(Parser.IngredientFromAppToDAL(ingredient), id);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }

            return NoContent();
        }

        [HttpDelete("Ingredients/{id}", Name = "DeleteIngredient")]
        public async Task<IActionResult> DeleteIngredient(int id)
        {
            var ingredientService = _mealsService.IngredientService;
            var ingredient = await ingredientService.GetItemById(id);

            if (ingredient == null)
            {
                return NotFound();
            }

            await ingredientService.DeleteItem(id);

            return NoContent();
        }

    }
}
