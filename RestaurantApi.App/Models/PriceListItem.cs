namespace RestaurantApi.App.Models
{
    public class PriceListItem
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public Meal? Meal { get; set; }
        public int MealId { get; set; }
        public int PortionId { get; set; }
        public Portion? Portion { get; set; }
    }
}
