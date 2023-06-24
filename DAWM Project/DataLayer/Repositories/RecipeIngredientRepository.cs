using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Domain.Models.MenuRelated;
using RestaurantAPI.Domain.RepositoriesAbstractions;

namespace DataLayer.Repositories
{
    public class RecipeIngredientRepository : RepositoryBase<RecipeIngredient>, IRecipeIngredientRepository
    {
        public RecipeIngredientRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<RecipeIngredient> GetRecordsWithIds(int recipieId, int ingredientId)
        {
            return await GetRecords()
                .Where(r => r.RecipeId == recipieId && r.IngredientId == ingredientId)
                .FirstOrDefaultAsync();
        }
    }
}
