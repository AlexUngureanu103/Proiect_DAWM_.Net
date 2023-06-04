using RestaurantAPI.Domain.Dtos.RecipeDtos;
using RestaurantAPI.Domain.Models.MenuRelated;

namespace RestaurantAPI.Domain.ServicesAbstractions
{
    public interface IRecipeService
    {
        Task<bool> AddRecipe(CreateOrUpdateRecipe recipe);

        Task<bool> DeleteRecipe(int recipeId);

        Task<bool> UpdateRecipe(int recipeId, CreateOrUpdateRecipe recipe);

        Task<Recipe> GetRecipe(int recipeId);

        Task<IEnumerable<Recipe>> GetAllRecipes();
    }
}
