
using Microsoft.EntityFrameworkCore;

using RestaurantApi.Dal.Exeptions;
using RestaurantApi.Dal.Models;

using System.Text;

namespace RestaurantApi.Dal.Repositories
{
    public class MealRepository : IMealRepository
    {
        private DbSet<Meal> _table;
        private RestaurantDbContext _context;

        public MealRepository(RestaurantDbContext context)
        {
            _table = context.Set<Meal>();
            _context = context;
        }

        public async Task<IEnumerable<Meal>> GetInIdRangeAsync(int startId, int endId)
        {
            return await _table.Where(item => item.Id >= startId && item.Id <= endId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Meal>> GetAllAsync()
        {
            return await _table.ToListAsync();
        }

        public async Task<Meal> GetByIdAsync(int id)
        {
            return await _table.FirstOrDefaultAsync(g => g.Id == id);
        }
        public async Task AddAsync(Meal entity)
        {
            ThrowIfNull(entity);
            var resMeal = new Meal()
            {
                Id = entity.Id,
                Description = entity.Description,
                Name = entity.Name,
            };
            if (entity.Ingredients is not null)
            {
                var matchingIngredients = GetMatchingIngredientsFromTable(entity);
                resMeal.Ingredients = matchingIngredients;
            }
           
            await _table.AddAsync(resMeal);
        }

        public async Task UpdateAsync(Meal entity, int id)
        {
            ThrowIfNull(entity);
            if (entity.Id != id)
            {
                throw new ArgumentException(nameof(entity), "entity.Id and id missmatch");
            }
            var tableEntity = await GetByIdAsync(id);
            tableEntity.Id = entity.Id;
            tableEntity.Description = entity.Description;
            tableEntity.Name = entity.Name;

            if (entity.Ingredients is not null)
            {
                var matchingIngredients = GetMatchingIngredientsFromTable(entity);
                tableEntity.Ingredients = matchingIngredients;
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            ThrowIfNull(entity);
            _table.Remove(entity);
        }
        private List<Ingredient> GetMatchingIngredientsFromTable(Meal entity)
        {
            var ingredientIds = entity.Ingredients!.Select(i => i.Id).ToList();

            return _context.Ingredients
                 .Where(i => ingredientIds.Contains(i.Id))
                 .ToList();
        }
        private static void ThrowIfNull(object entity)
        {
            if (entity is null)
            {
                throw new EntityNotFoundException("Entity with the specified ID not found.");
            }
        }
    }
}
