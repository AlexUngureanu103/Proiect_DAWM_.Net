using RestaurantAPI.Domain.Models.MenuRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Domain.Dtos.RecipeDtos
{
    public class RecipeInfo
    {
        public string Name { get; set; }

        public float Price { get; set; }

        public int DishesTypeId { get; set; }

        public List<IngredientAndWeight> IngredientIdAndWeight { get; set; }
    }
}
