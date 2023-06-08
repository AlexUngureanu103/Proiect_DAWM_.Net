using RestaurantAPI.Domain.Dtos.RecipeDtos;
using RestaurantAPI.Domain.Models.MenuRelated;

namespace RestaurantAPI.Domain.Mapping
{
    public static class RecipeMapping
    {
        public static Recipe MapToRecipe(CreateOrUpdateRecipe recipe)
        {
            if (recipe == null)
                return null;

            return new Recipe
            {
                DishesType = recipe.DishesType,
                DishesTypeId = recipe.DishesTypeId,
                Name = recipe.Name,
                Price = recipe.Price
            };
        }
    }
}
