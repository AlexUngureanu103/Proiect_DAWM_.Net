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
                DishesTypeId = recipe.DishesTypeId,
                Name = recipe.Name,
                Price = recipe.Price,
                ImageUrl = recipe.ImageUrl
            };
        }

        public static RecipeInfo MapToRecipeInfo(Recipe recipe)
        {
            if (recipe == null)
                return null;
            return new RecipeInfo
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Price = recipe.Price,
                DishesTypeId = recipe.DishesTypeId,
                ImageUrl = recipe.ImageUrl,
                RecipeIds = recipe.Ingredients.Select(r => r.IngredientId).ToList()
            };
        }
    }
}
