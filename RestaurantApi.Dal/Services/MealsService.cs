
using RestaurantApi.Dal.Models;

namespace RestaurantApi.Dal.Services
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


    public class OrdersService : IOrdersService
    {
        
        public ICRUDService<Order> OrderService { get; set; }
        public IOrderItemCRUDService OrderItemService { get; set; }

        public OrdersService(ICRUDService<Order> orderService,
                            IOrderItemCRUDService orderItemService
            )
        {
            OrderService = orderService;
            OrderItemService = orderItemService;
        }
    }
}
