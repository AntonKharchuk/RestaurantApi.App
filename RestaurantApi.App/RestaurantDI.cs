using Microsoft.EntityFrameworkCore;

using RestaurantApi.Dal;
using RestaurantApi.Dal.Models;
using RestaurantApi.Dal.Repositories;
using RestaurantApi.Business.Services;

namespace RestaurantApi.App
{

    namespace RestaurantApi.App
    {
        public static class RestaurantDI
        {
            public static void ConfigureRestaurantServices(this IServiceCollection services)
            {
                RegisterModelServices<Ingredient>(services);

                RegisterModelServices<Portion>(services);
                RegisterModelServices<PriceListItem>(services);
                RegisterModelServices<Order>(services);
                RegisterModelServices<OrderItem>(services);

                services.AddTransient<IMealRepository, MealRepository>();
                services.AddTransient<IMealCRUDService, MealCRUDService>();

                services.AddTransient<IOrderItemRepository, OrderItemRepository>();
                services.AddTransient<IOrderItemCRUDService, OrderItemCRUDService>();

            }

            static void  RegisterModelServices<T>(IServiceCollection services) where T : BaseEntity
            {
                services.AddTransient<IRepository<T>, Repository<T>>();
                services.AddTransient<ICRUDService<T>, CRUDService<T>>();
            }
        }
    }

}
