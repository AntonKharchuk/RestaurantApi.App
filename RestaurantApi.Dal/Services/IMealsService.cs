
using RestaurantApi.Dal.Models;

namespace RestaurantApi.Dal.Services
{
    public interface IMealsService
    {
        ICRUDService<Ingredient> IngredientService { get; }
        IMealCRUDService MealService { get; }
        ICRUDService<Portion> PortionService { get; }
        ICRUDService<PriceListItem> PriceListService { get; }
    }
    public interface IOrdersService
    {
        ICRUDService<Order> OrderService { get; }
        IOrderItemCRUDService OrderItemService { get; }
    }
}
