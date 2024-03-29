﻿namespace RestaurantAPI.Domain.Dtos.RecipeDtos
{
    public class RecipeInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public float Price { get; set; }

        public int DishesTypeId { get; set; }

        public List<IngredientAndWeight> IngredientIdAndWeight { get; set; }

        public string ImageUrl { get; set; }
    }
}
