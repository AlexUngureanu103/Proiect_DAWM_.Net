using RestaurantAPI.Domain.Models.MenuRelated;

namespace RestaurantAPI.Domain.ServicesAbstractions
{
    public interface IIngredientsService
    {
        Task<bool> AddIngredient(Ingredient ingredient);

        Task<bool> DeleteIngredient(int ingredientId);

        Task<bool> UpdateIngredient(int ingredientId, Ingredient ingredient);

        Task<Ingredient> GetIngredient(int ingredientId);

        Task<IEnumerable<Ingredient>> GetAllIngredients();
    }
}
