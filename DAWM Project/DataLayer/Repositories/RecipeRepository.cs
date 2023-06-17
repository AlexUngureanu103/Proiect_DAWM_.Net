using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Domain.Models.MenuRelated;
using RestaurantAPI.Domain.RepositoriesAbstractions;
using RestaurantAPI.Exceptions;

namespace DataLayer.Repositories
{
    public class RecipeRepository : RepositoryBase<Recipe>, IRecipeRepository
    {
        public RecipeRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Recipe>> GetAllWithIngredients()
        {
            return await GetRecords().Include(r => r.Ingredients).ToListAsync();
        }

        public new async Task<Recipe> GetByIdAsync(int entityId)
        {
            var resultFromDb = await GetRecords().Include(r => r.Ingredients).Where(r => r.Id == entityId).FirstOrDefaultAsync();

            return resultFromDb;
        }

        public new async Task UpdateAsync(int entityId, Recipe entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var entityFromDb = await GetByIdAsync(entityId);
            if (entityFromDb == null)
                throw new EntityNotFoundException($"{nameof(Recipe)} with id {entity.Id} does not exist.");

            entityFromDb.Name = entity.Name;
            entityFromDb.DishesTypeId = entity.DishesTypeId;
            entityFromDb.Price = entity.Price;
        }
    }
}
