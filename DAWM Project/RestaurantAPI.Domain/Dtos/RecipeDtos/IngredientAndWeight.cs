using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Domain.Dtos.RecipeDtos
{
    public class IngredientAndWeight
    {
        public int IngredientId { get; set; }
        public double Weight { get; set; }
    }
}
