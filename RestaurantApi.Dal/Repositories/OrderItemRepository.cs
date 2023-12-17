
using Microsoft.EntityFrameworkCore;

using RestaurantApi.Dal.Models;

namespace RestaurantApi.Dal.Repositories
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        private DbSet<OrderItem> _table;
        private RestaurantDbContext _context;

        public OrderItemRepository(RestaurantDbContext context) : base(context)
        {
            _table = context.Set<OrderItem>();
            _context = context;
        }


        public async Task<IEnumerable<OrderItem>> GetByOrderId(int id, string includes = "")
        {
            var query = _table.AsQueryable();

            if (!string.IsNullOrEmpty(includes))
            {
                foreach (var include in includes.Split(","))
                {
                    query = query.Include(include);
                }
            }

            return await query.Where(g => g.OrderId == id).ToListAsync();
        }
    }
}
