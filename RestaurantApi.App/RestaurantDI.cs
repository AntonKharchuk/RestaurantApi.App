using Microsoft.EntityFrameworkCore;

using RestaurantApi.Dal;
using RestaurantApi.Dal.Models;
using RestaurantApi.Dal.Repositories;
using RestaurantApi.Dal.Services;

namespace RestaurantApi.App
{

    namespace RestaurantApi.App
    {
        public class RestaurantDI
        {
            public void ConfigureServices(IServiceCollection services)
            {
                RegisterModelServices<Ingredient>(services);

                services.AddTransient<IMealRepository, MealRepository>();
                services.AddTransient<IMealCRUDService, MealCRUDService>();

                //services.AddTransient<IOrderItemRepository, OrderItemRepository>();
                //services.AddTransient<IOrderItemCRUDService, OrderItemCRUDService>();

                RegisterModelServices<Portion>(services);
                RegisterModelServices<PriceListItem>(services);
                RegisterModelServices<Order>(services);
                RegisterModelServices<OrderItem>(services);

                services.AddTransient<IMealsService, MealsService>();
                //builder.Services.AddTransient<IOrdersService, OrdersService>();
            }

            void RegisterModelServices<T>(IServiceCollection services) where T : class
            {
                services.AddTransient<IRepository<T>, Repository<T>>();
                services.AddTransient<ICRUDService<T>, CRUDService<T>>();
            }
            public void AddDataToDB(RestaurantDbContext dbContext)
            {
                var chiken = new Ingredient { Id = 1, Name = "Chicken" };
                var rice = new Ingredient { Id = 2, Name = "Rice" };
                var broccolli = new Ingredient { Id = 3, Name = "Broccoli" };
                var bellPaper = new Ingredient { Id = 6, Name = "Bell Pepper" };

                // Meal 1
                dbContext.Meals.Add(
                    new Meal
                    {
                        Id = 1,
                        Name = "Grilled Chicken Bowl",
                        Description = "A delicious bowl with grilled chicken, rice, and vegetables.",
                        Ingredients = new List<Ingredient>
                        {
                        chiken,rice,broccolli
                        }
                    }
                );

                // Meal 2
                dbContext.Meals.Add(
                    new Meal
                    {
                        Id = 2,
                        Name = "Vegetarian Pasta",
                        Description = "A tasty pasta dish with assorted vegetables.",
                        Ingredients = new List<Ingredient>
                        {
                        new Ingredient { Id = 4, Name = "Pasta" },
                        new Ingredient { Id = 5, Name = "Tomatoes" },
                        bellPaper
                        }
                    }
                );

                // Meal 3
                dbContext.Meals.Add(
                    new Meal
                    {
                        Id = 3,
                        Name = "Salmon Salad",
                        Description = "A refreshing salad with grilled salmon and greens.",
                        Ingredients = new List<Ingredient>
                        {
                        new Ingredient { Id = 7, Name = "Salmon" },
                        new Ingredient { Id = 8, Name = "Mixed Greens" },
                        new Ingredient { Id = 9, Name = "Cucumber" },
                        }
                    }
                );
                // Meal 4
                dbContext.Meals.Add(
                    new Meal
                    {
                        Id = 4,
                        Name = "Margherita Pizza",
                        Description = "Classic pizza topped with tomato, mozzarella, and basil.",
                        Ingredients = new List<Ingredient>
                        {
                new Ingredient { Id = 10, Name = "Pizza Dough" },
                new Ingredient { Id = 11, Name = "Tomato Sauce" },
                new Ingredient { Id = 12, Name = "Mozzarella" },
                new Ingredient { Id = 13, Name = "Basil" },
                        }
                    }
                );

                // Meal 5
                dbContext.Meals.Add(
                    new Meal
                    {
                        Id = 5,
                        Name = "Beef Stir-Fry",
                        Description = "Savory stir-fried beef with vegetables and soy sauce.",
                        Ingredients = new List<Ingredient>
                        {
                new Ingredient { Id = 14, Name = "Beef Strips" },
                broccolli,
                bellPaper,
                new Ingredient { Id = 17, Name = "Soy Sauce" },
                        }
                    }
                );

                Portion smallPortion = new Portion
                {
                    Id = 1,
                    Name = "Small"
                };

                Portion mediumPortion = new Portion
                {
                    Id = 2,
                    Name = "Medium"
                };

                Portion largePortion = new Portion
                {
                    Id = 3,
                    Name = "Large"
                };


                for (int i = 0; i < 5; i++)
                {
                    CreatePriceListForMeal(1 + i, 1, 10.99m + 2 * i, smallPortion);
                    CreatePriceListForMeal(1 + i, 2, 15.99m + 2 * i, mediumPortion);
                    CreatePriceListForMeal(1 + i, 3, 20.99m + 2 * i, largePortion);
                }
                dbContext.SaveChanges();

                Order order1 = CreateOrder("John Doe", new DateTime(2023, 11, 1));
                Order order2 = CreateOrder("Jane Smith", new DateTime(2023, 11, 5));
                Order order3 = CreateOrder("Bob Johnson", new DateTime(2023, 11, 10));
                Order order4 = CreateOrder("Alice Brown", new DateTime(2023, 11, 15));
                Order order5 = CreateOrder("Charlie White", new DateTime(2023, 11, 20));

                dbContext.Orders.AddRange(order1, order2, order3, order4, order5);

                dbContext.SaveChanges();

                CreateOrderItems(order1, 3);
                CreateOrderItems(order2, 2);
                CreateOrderItems(order3, 4);
                CreateOrderItems(order4, 1);
                CreateOrderItems(order5, 3);
                // Save changes to the database
                dbContext.SaveChanges();

                void CreatePriceListForMeal(int mealId, int portionId, decimal price, Portion portion)
                {
                    var priceListItem = new PriceListItem
                    {
                        MealId = mealId,
                        PortionId = portionId,
                        Price = price,
                        Amount = 1, // You can set the amount as needed
                        Portion = portion
                    };

                    dbContext.PriceList.Add(priceListItem);
                }
                Order CreateOrder(string customerName, DateTime date)
                {
                    return new Order
                    {
                        CustomerName = customerName,
                        Date = date
                    };
                }
                void CreateOrderItems(Order order, int numberOfItems)
                {
                    var PriceList = dbContext.PriceList.ToList();
                    Random random = new Random();

                    for (int i = 1; i <= numberOfItems; i++)
                    {
                        var orderItem = new OrderItem
                        {
                            Coment = $"Item {i} for order {order.Id}",
                            IsReady = random.Next(2) == 1 ? true : false,
                            Order = order,
                            PriceListItem = PriceList[random.Next(PriceList.Count)] // Implement this method to get a random PriceListItem
                        };

                        dbContext.OrderItems.Add(orderItem);
                    }
                }

            }
        }
    }

}
