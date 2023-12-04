using RestaurantApi.Dal.Models;
using RestaurantApi.Dal.Repositories;
using RestaurantApi.Dal.Services;

namespace RestaurantApi.App
{
    public class RestaurantDI
    {
        public void ConfigureServices(IServiceCollection services)
        {
            RegisterModelServices<Ingredient>(services);

            //services.AddTransient<IMealRepository, MealRepository>();
            //services.AddTransient<IMealCRUDService, MealCRUDService>();

            //services.AddTransient<IOrderItemRepository, OrderItemRepository>();
            //services.AddTransient<IOrderItemCRUDService, OrderItemCRUDService>();

            RegisterModelServices<Portion>(services);
            RegisterModelServices<PriceListItem>(services);
            RegisterModelServices<Order>(services);
            RegisterModelServices<OrderItem>(services);
        }

        void RegisterModelServices<T>(IServiceCollection services) where T : class
        {
            services.AddTransient<IRepository<T>, Repository<T>>();
            services.AddTransient<ICRUDService<T>, CRUDService<T>>();
        }
    }
}
