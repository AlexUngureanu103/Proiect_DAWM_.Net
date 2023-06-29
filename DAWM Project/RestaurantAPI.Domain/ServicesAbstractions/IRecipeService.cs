using RestaurantAPI.Domain.Dtos.RecipeDtos;
using RestaurantAPI.Domain.Models.MenuRelated;

namespace RestaurantAPI.Domain.ServicesAbstractions
{
    public interface IRecipeService
    {
        Task<int> Create(CreateOrUpdateRecipe recipe);
        
        Task<bool> Delete(int recipeId);

        Task<bool> Update(int recipeId, CreateOrUpdateRecipe recipe);

        Task<bool> AddIngredient(int recipeId, int ingredientId);

        Task<bool> DeleteIngredient(int recipeId, int ingredientId);

        Task<RecipeInfo> GetById(int recipeId);

        Task<IEnumerable<RecipeInfo>> GetAll();
    }
}
