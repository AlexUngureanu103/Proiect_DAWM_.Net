using RestaurantAPI.Domain.Models.MenuRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Domain.RepositoriesAbstractions
{
    public interface IRecipeIngredientRepository : IRepository<RecipeIngredient>
    {
        public Task<RecipeIngredient> GetRecordsWithIds(int recipieId, int ingredientId);
    }
}
