using RestaurantAPI.Domain;
using RestaurantAPI.Domain.Dtos.RecipeDtos;
using RestaurantAPI.Domain.Mapping;
using RestaurantAPI.Domain.Models.MenuRelated;
using RestaurantAPI.Domain.ServicesAbstractions;
using RestaurantAPI.Exceptions;

namespace Core.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IDataLogger logger;

        public RecipeService(IUnitOfWork unitOfWork, IDataLogger logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public async Task<bool> AddRecipe(CreateOrUpdateRecipe recipe)
        {
            if (recipe == null)
                throw new ArgumentNullException(nameof(recipe));

            Recipe recipeData = RecipeMapping.MapToRecipe(recipe);

            await _unitOfWork.RecipeRepository.AddAsync(recipeData);

            bool result = await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<bool> DeleteRecipe(int recipeId)
        {
            try
            {
                await _unitOfWork.RecipeRepository.DeleteAsync(recipeId);
            }
            catch (EntityNotFoundException exception)
            {
                logger.LogError(exception.Message, exception);

                return false;
            }

            bool response = await _unitOfWork.SaveChangesAsync();

            return response;
        }

        public async Task<IEnumerable<Recipe>> GetAllRecipes()
        {
            var recipesFrombDb = await _unitOfWork.RecipeRepository.GetAllAsync();

            return recipesFrombDb;
        }

        public async Task<Recipe> GetRecipe(int recipeId)
        {
            var recipeFromDb = await _unitOfWork.RecipeRepository.GetByIdAsync(recipeId);

            return recipeFromDb;
        }

        public async Task<bool> UpdateRecipe(int recipeId, CreateOrUpdateRecipe recipe)
        {
            if (recipe == null)
                throw new ArgumentNullException(nameof(recipe));

            Recipe recipeData = RecipeMapping.MapToRecipe(recipe);
            await _unitOfWork.RecipeRepository.UpdateAsync(recipeId, recipeData);

            bool response = await _unitOfWork.SaveChangesAsync();

            return response;
        }
    }
}
