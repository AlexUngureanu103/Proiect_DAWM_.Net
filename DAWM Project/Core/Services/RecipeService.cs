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

        public async Task<bool> AddIngredient(int recipeId, int ingredientId, double weight)
        {
            
            if (await _unitOfWork.IngredientRepository.GetByIdAsync(ingredientId) == null) 
            {
                logger.LogInfo($"[AddRecipeIngredient] Ingredient with id {ingredientId} not found");
                return false;
            }
            if (await _unitOfWork.RecipeRepository.GetByIdAsync(recipeId) == null)
            {
                logger.LogInfo($"[AddRecipeIngredient] Recipe with id {recipeId} not found");
                return false;
            }

            await _unitOfWork.RecipeIngredientRepository.AddAsync(new RecipeIngredient { RecipeId = recipeId, IngredientId = ingredientId, Weight = weight });

            bool result = await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<bool> Create(CreateOrUpdateRecipe recipe)
        {
            if (recipe == null)
            {
                logger.LogError($"Null argument from controller: {nameof(recipe)}");
                throw new ArgumentNullException(nameof(recipe));
            }
            if (await _unitOfWork.DishesTypeRepository.GetByIdAsync(recipe.DishesTypeId) == null)
            {
                logger.LogWarn($"DishesType with id {recipe.DishesTypeId} not found");
                return false;
            }

            Recipe recipeData = RecipeMapping.MapToRecipe(recipe);

            await _unitOfWork.RecipeRepository.AddAsync(recipeData);

            bool result = await _unitOfWork.SaveChangesAsync();
            logger.LogInfo($"Recipe with id {recipeData.Id} added");

            return result;
        }

        public async Task<bool> Delete(int recipeId)
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
            logger.LogInfo($"Recipe with id {recipeId} deleted");

            return response;
        }

        public async Task<bool> DeleteIngredient(int recipeId, int ingredientId)
        {
            RecipeIngredient recipeIngredient = await _unitOfWork.RecipeIngredientRepository.GetRecordsWithIds(recipeId, ingredientId);
            if (recipeIngredient == null)
            {
                logger.LogInfo($"[DeleteRecipeIngredient] item to delete not found");
                return false;
            }

            await _unitOfWork.RecipeIngredientRepository.DeleteAsync(recipeIngredient.Id);

            bool result = await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<IEnumerable<RecipeInfo>> GetAll()
        {
            var recipesFrombDb = await _unitOfWork.RecipeRepository.GetAllWithIngredients();

            return recipesFrombDb.Select(recipe => RecipeMapping.MapToRecipeInfo(recipe)).ToList();
        }

        public async Task<RecipeInfo> GetById(int recipeId)
        {
            var recipeFromDb = await _unitOfWork.RecipeRepository.GetByIdAsync(recipeId);

            return RecipeMapping.MapToRecipeInfo(recipeFromDb);
        }

        public async Task<bool> Update(int recipeId, CreateOrUpdateRecipe recipe)
        {
            if (recipe == null)
            {
                logger.LogError($"Null argument from controller: {nameof(recipe)}");

                throw new ArgumentNullException(nameof(recipe));
            }
            if (await _unitOfWork.DishesTypeRepository.GetByIdAsync(recipe.DishesTypeId) == null)
            {
                logger.LogWarn($"DishesType with id {recipe.DishesTypeId} not found");
                return false;
            }

            Recipe recipeData = RecipeMapping.MapToRecipe(recipe);

            try
            {
                await _unitOfWork.RecipeRepository.UpdateAsync(recipeId, recipeData);
            }
            catch (EntityNotFoundException exception)
            {
                logger.LogError(exception.Message, exception);

                return false;
            }
            catch (ArgumentNullException exception)
            {
                logger.LogError(exception.Message, exception);
                return false;
            }

            bool response = await _unitOfWork.SaveChangesAsync();
            logger.LogInfo($"Recipe with id {recipeId} updated");

            return response;
        }
    }
}
