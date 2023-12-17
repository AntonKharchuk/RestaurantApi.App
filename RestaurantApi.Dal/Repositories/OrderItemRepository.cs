
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


        public async Task<IEnumerable<OrderItem>> GetByOrderId(int id, string include = "")
        {
            var query = _table.AsQueryable();

            if (!string.IsNullOrEmpty(include))
            {
                foreach (var field in include.Split(","))
                {
                    query = query.Include(field);
                }
            }

            return await query.Where(g => g.OrderId == id).ToListAsync();
        }
    }
}
