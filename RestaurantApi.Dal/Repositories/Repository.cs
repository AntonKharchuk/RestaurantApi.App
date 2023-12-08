
using Microsoft.EntityFrameworkCore;

using RestaurantApi.Dal.Exeptions;
using RestaurantApi.Dal.Models;

namespace RestaurantApi.Dal.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly RestaurantDbContext _context;

        private DbSet<T> _table;

        public Repository(RestaurantDbContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _table.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _table.FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task AddAsync(T entity)
        {
            ThrowIfNull(entity);
            await _table.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity, int id)
        {
            ThrowIfNull(entity);
            if (entity.Id!=id)
            {
                throw new ArgumentException(nameof(entity), "entity.Id and id missmatch");
            }
            var tableEntity = await GetByIdAsync(id);
            ThrowIfNull(tableEntity);
            _context.Entry(tableEntity).CurrentValues.SetValues(entity);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var tableEntity = await GetByIdAsync(id);
            ThrowIfNull(tableEntity);
            _table.Remove(tableEntity);
        }
        private static void ThrowIfNull(T? entity)
        {
            if (entity is null)
            {
                throw new EntityNotFoundException("Entity with the specified ID not found.");
            }
        }
    }
}
