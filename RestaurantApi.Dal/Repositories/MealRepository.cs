
using Microsoft.EntityFrameworkCore;
using RestaurantApi.Dal.Models;

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

        public async Task<IEnumerable<Meal>> GetInIdRange(int startId, int endId)
        {
            return await _table.Where(item => item.Id >= startId && item.Id <= endId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Meal>> GetAll()
        {
            return await _table.ToListAsync();
        }

        public async Task<Meal> GetById(int id)
        {
            var entity = await _table.FindAsync(id);
            ThrowIfNull(entity!);
            return entity!;
        }
        public async Task Add(Meal entity)
        {
            ThrowIfNull(entity);

            var matchingIngredients = GetMatchingIngredientsFromTable(entity);

            var resMeal = new Meal()
            {
                Id = entity.Id,
                Description = entity.Description,
                Name = entity.Name,
                Ingredients = matchingIngredients,
            };

            await _table.AddAsync(resMeal);
        }

        public async Task Update(Meal entity, int id)
        {
            ThrowIfNull(entity);
            var matchingIngredients = GetMatchingIngredientsFromTable(entity);

            var tableEntity = await GetById(id);
            tableEntity.Id = entity.Id;
            tableEntity.Description = entity.Description;
            tableEntity.Name = entity.Name;
            tableEntity.Ingredients = matchingIngredients;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            var entity = await _table.FindAsync(id);
            ThrowIfNull(entity!);
            _table.Remove(entity!);
        }
        private List<Ingredient> GetMatchingIngredientsFromTable(Meal entity)
        {
            ThrowIfNull(entity.Ingredients!);
            var ingredientIds = entity.Ingredients!.Select(i => i.Id).ToList();

            return _context.Ingredients
                 .Where(i => ingredientIds.Contains(i.Id))
                 .ToList();
        }
        private static void ThrowIfNull(object entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity with the specified ID not found.");
            }
        }
    }
}
