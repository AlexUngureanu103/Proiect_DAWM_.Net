using RestaurantAPI.Domain.Dtos.IngredientDtos;
using RestaurantAPI.Domain.Models.MenuRelated;

namespace RestaurantAPI.Domain.ServicesAbstractions
{
    public interface IIngredientsService
    {
        Task<bool> Create(CreateOrUpdateIngredient ingredient);

        Task<bool> Delete(int ingredientId);

        Task<bool> Update(int ingredientId, CreateOrUpdateIngredient ingredient);

        Task<Ingredient> GetById(int ingredientId);

        Task<IEnumerable<Ingredient>> GetAll();
    }
}
