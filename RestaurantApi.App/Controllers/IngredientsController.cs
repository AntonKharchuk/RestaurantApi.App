using Microsoft.AspNetCore.Mvc;

using RestaurantApi.App.Models;
using RestaurantApi.Business.Services;

namespace RestaurantApi.App.Controllers
{

    [Route("/api/Meal/[controller]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        private ICRUDService<Dal.Models.Ingredient> _ingredientsServise;

        public IngredientsController(ICRUDService<Dal.Models.Ingredient> ingredientsServise)
        {
            _ingredientsServise = ingredientsServise;
        }
        [HttpGet]
        public async Task<IEnumerable<Ingredient>> GetAllIngredients()
        {
            var asyncResult = _ingredientsServise.GetAllItemsAsync();

            var result = new List<Ingredient>();

            foreach (var item in await asyncResult)
            {
                result.Add(Parser.IngredientFromDALToApp(item));
            }

            return result;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Ingredient>> GetIngredientById(int id)
        {
            var ingredient = await _ingredientsServise.GetItemByIdAsync(id);
            if (ingredient is null)
                return NotFound();

            return Parser.IngredientFromDALToApp(ingredient);
        }

        [HttpPost]
        public async Task<ActionResult<Ingredient>> CreateIngredient([FromBody] Ingredient ingredient)
        {
            await _ingredientsServise.CreateItemAsync(Parser.IngredientFromAppToDAL(ingredient));

            return Ok(ingredient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIngredient(int id, [FromBody] Ingredient ingredient)
        {
            await _ingredientsServise.UpdateItemAsync(Parser.IngredientFromAppToDAL(ingredient), id);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngredient(int id)
        {
            await _ingredientsServise.DeleteItemAsync(id);

            return Ok();
        }

    }
}
