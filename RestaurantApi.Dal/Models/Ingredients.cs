
namespace RestaurantApi.Dal.Models
{
    public class Ingredient: BaseEntity
    {
        public string? Name { get; set; }
        public List<Meal>? Meals { get; set; }

    }
    public class Meal: BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<Ingredient>? Ingredients { get; set; }
    }

    public class Portion: BaseEntity
    {
        public string? Name { get; set; }
    }
    public class PriceListItem: BaseEntity
    {
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public Meal? Meal { get; set; }
        public int MealId { get; set; }
        public int PortionId { get; set; }
        public Portion? Portion { get; set; }
    }
    public class Order: BaseEntity
    {
        public string? CustomerName { get; set; }
        public DateTime Date { get; set; }
    }
    public class OrderItem: BaseEntity
    {
        public string? Coment { get; set; }
        public bool IsReady { get; set; }
        public Order? Order { get; set; }
        public int OrderId { get; set; }
        public PriceListItem? PriceListItem;
        public int PriceListItemId { get; set; }
    }
}
