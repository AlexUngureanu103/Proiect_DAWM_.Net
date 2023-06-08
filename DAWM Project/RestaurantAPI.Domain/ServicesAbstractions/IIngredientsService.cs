using RestaurantAPI.Domain.Dtos.IngredientDtos;
using RestaurantAPI.Domain.Models.MenuRelated;

namespace RestaurantAPI.Domain.ServicesAbstractions
{
    public interface IIngredientsService
    {
        Task<bool> AddIngredient(CreateOrUpdateIngredient ingredient);

        Task<bool> DeleteIngredient(int ingredientId);

        Task<bool> UpdateIngredient(int ingredientId, CreateOrUpdateIngredient ingredient);

        Task<Ingredient> GetIngredient(int ingredientId);

        Task<IEnumerable<Ingredient>> GetAllIngredients();
    }
}
