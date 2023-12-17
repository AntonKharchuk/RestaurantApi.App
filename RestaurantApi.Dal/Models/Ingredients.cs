﻿
namespace RestaurantApi.Dal.Models
{
    public class Ingredient: BaseEntity
    {
        public string? Name { get; set; }
        public List<Meal>? Meals { get; set; }
    }
}
