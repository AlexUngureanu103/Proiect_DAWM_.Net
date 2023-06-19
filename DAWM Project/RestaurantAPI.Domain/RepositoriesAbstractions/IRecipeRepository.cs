using RestaurantAPI.Domain.Models.MenuRelated;

namespace RestaurantAPI.Domain.RepositoriesAbstractions
{
    public interface IRecipeRepository : IRepository<Recipe>
    {
        public Task<IEnumerable<Recipe>> GetAllWithIngredients();

        public Task<Recipe> GetByIdAsync(int entityId);
    }
}
