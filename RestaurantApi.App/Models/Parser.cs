﻿using RestaurantApi.Dal.Models;

namespace RestaurantApi.App.Models
{
    public static class Parser
    {
        public static Ingredient IngredientFromDALToApp(Dal.Models.Ingredient ingredient)
        {
            return new Ingredient()
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
            };
        }
        public static Meal MealFromDALToApp(Dal.Models.Meal meal)
        {
            var result = new Meal()
            {
                Id = meal.Id,
                Name = meal.Name,
                Description = meal.Description,
                Ingredients = new List<Ingredient> { }
            };
            if (meal.Ingredients is not null)
                foreach (var item in meal.Ingredients)
                {
                    result.Ingredients.Add(IngredientFromDALToApp(item));
                }

            return result;
        }

        public static Portion PortionFromDALToApp(Dal.Models.Portion portion)
        {
            return new Portion()
            {
                Id = portion.Id,
                Name = portion.Name,
            };
        }

        public static PriceListItem PriceListItemFromDALToApp(Dal.Models.PriceListItem priceListItem)
        {
            return new PriceListItem()
            {
                Id = priceListItem.Id,
                Price = priceListItem.Price,
                Amount = priceListItem.Amount,
                MealId = priceListItem.MealId,
                PortionId = priceListItem.PortionId,
                Meal = MealFromDALToApp(priceListItem.Meal!),
                Portion = PortionFromDALToApp(priceListItem.Portion!),
            };
        }

        public static Order OrderFromDALToApp(Dal.Models.Order order)
        {
            return new Order()
            {
                Id = order.Id,
                CustomerName = order.CustomerName,
                Date = order.Date,
            };
        }

        public static OrderItem OrderItemFromDALToApp(Dal.Models.OrderItem orderItem)
        {
            return new OrderItem()
            {
                Id = orderItem.Id,
                Coment = orderItem.Coment,
                IsReady = orderItem.IsReady,
                OrderId = orderItem.OrderId,
                PriceListItemId = orderItem.PriceListItemId,
                Order = OrderFromDALToApp(orderItem.Order!),
                PriceListItem = PriceListItemFromDALToApp(orderItem.PriceListItem!),
            };
        }

        public static Dal.Models.Ingredient IngredientFromAppToDAL(Ingredient ingredient)
        {
            return new Dal.Models.Ingredient()
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
            };
        }

        public static Dal.Models.Meal MealFromAppToDAL(Meal meal)
        {
            var result = new Dal.Models.Meal()
            {
                Id = meal.Id,
                Name = meal.Name,
                Description = meal.Description,
                Ingredients = new List<Dal.Models.Ingredient> { }
            };
            if (meal.Ingredients is not null)
                foreach (var item in meal.Ingredients)
                {
                    result.Ingredients.Add(IngredientFromAppToDAL(item));
                }

            return result;
        }

        public static Dal.Models.Portion PortionFromAppToDAL(Portion portion)
        {
            return new Dal.Models.Portion()
            {
                Id = portion.Id,
                Name = portion.Name,
            };
        }

        public static Dal.Models.PriceListItem PriceListItemFromAppToDAL(PriceListItem priceListItem)
        {
            return new Dal.Models.PriceListItem()
            {
                Id = priceListItem.Id,
                Price = priceListItem.Price,
                Amount = priceListItem.Amount,
                MealId = priceListItem.MealId,
                PortionId = priceListItem.PortionId,
                Meal = null,
                Portion = null,
            };
        }

        public static Dal.Models.Order OrderFromAppToDAL(Order order)
        {
            return new Dal.Models.Order()
            {
                Id = order.Id,
                CustomerName = order.CustomerName,
                Date = order.Date,
            };
        }

        public static Dal.Models.OrderItem OrderItemFromAppToDAL(OrderItem orderItem)
        {
            return new Dal.Models.OrderItem()
            {
                Id = orderItem.Id,
                Coment = orderItem.Coment,
                IsReady = orderItem.IsReady,
                OrderId = orderItem.OrderId,
                PriceListItemId = orderItem.PriceListItemId,
                Order = null,
                PriceListItem = null,
            };
        }
    }
}
