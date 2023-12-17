namespace RestaurantApi.App.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
    public class Meal
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<Ingredient>? Ingredients { get; set; }
    }

    public class Portion
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
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
    public class Order
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public DateTime Date { get; set; }
    }
    public class OrderItem
    {
        public int Id { get; set; }
        public string? Coment { get; set; }
        public bool IsReady { get; set; }
        public Order? Order { get; set; }
        public int OrderId { get; set; }
        public PriceListItem? PriceListItem { get; set; }
        public int PriceListItemId { get; set; }
    }
}
