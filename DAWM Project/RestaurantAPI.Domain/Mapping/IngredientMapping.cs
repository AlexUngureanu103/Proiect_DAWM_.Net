using RestaurantAPI.Domain.Dtos.IngredientDtos;
using RestaurantAPI.Domain.Models.MenuRelated;

namespace RestaurantAPI.Domain.Mapping
{
    public static class IngredientMapping
    {
        public static Ingredient MapToIngredient(CreateOrUpdateIngredient ingredient)
        {
            if (ingredient == null)
                return null;
            return new Ingredient
            {
                Name = ingredient.Name,
            };
        }
    }
}
