
using Microsoft.EntityFrameworkCore;

namespace RestaurantApi.Dal.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly RestaurantDbContext _context;

        private DbSet<T> _table;

        public Repository(RestaurantDbContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _table.ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            var entity = await _table.FindAsync(id);
            ThrowIfNull(entity);
            return entity!;
        }

        public async Task Add(T entity)
        {
            ThrowIfNull(entity);
            await _table.AddAsync(entity);
        }

        public async Task Update(T entity, int id)
        {
            var tableEntity = await GetById(id);
            _context.Entry(tableEntity).CurrentValues.SetValues(entity);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            var entity = await _table.FindAsync(id);
            ThrowIfNull(entity);
            _table.Remove(entity!);
        }
        private static void ThrowIfNull(T? entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity with the specified ID not found.");
            }
        }
    }
}
