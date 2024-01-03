
using RestaurantApi.Dal.Models;

namespace RestaurantApi.Business.Services
{
    public class MealsService : IMealsService
    {
        public ICRUDService<Ingredient> IngredientService { get; set; }
        public IMealCRUDService MealService { get; set; }
        public ICRUDService<Portion> PortionService { get; set; }
        public ICRUDService<PriceListItem> PriceListService { get; set; }


        public MealsService(ICRUDService<Ingredient> ingredientService,
                            IMealCRUDService mealService,
                            ICRUDService<Portion> portionService,
                            ICRUDService<PriceListItem> priceListService
            )
        {
            IngredientService = ingredientService;
            MealService = mealService;
            PortionService = portionService;
            PriceListService = priceListService;

        }
    }
}
