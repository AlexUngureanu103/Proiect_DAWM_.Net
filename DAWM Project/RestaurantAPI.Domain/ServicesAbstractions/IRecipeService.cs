using RestaurantAPI.Domain.Dtos.RecipeDtos;
using RestaurantAPI.Domain.Models.MenuRelated;

namespace RestaurantAPI.Domain.ServicesAbstractions
{
    public interface IRecipeService
    {
        Task<bool> Create(CreateOrUpdateRecipe recipe);

        Task<bool> Delete(int recipeId);

        Task<bool> Update(int recipeId, CreateOrUpdateRecipe recipe);

        Task<Recipe> GetById(int recipeId);

        Task<IEnumerable<Recipe>> GetAll();
    }
}
