
namespace RestaurantApi.Dal.Models
{
    public class PriceListItem: BaseEntity
    {
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public Meal? Meal { get; set; }
        public int MealId { get; set; }
        public int PortionId { get; set; }
        public Portion? Portion { get; set; }
    }
}
