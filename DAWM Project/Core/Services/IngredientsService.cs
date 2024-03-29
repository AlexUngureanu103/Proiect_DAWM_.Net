﻿using RestaurantAPI.Domain;
using RestaurantAPI.Domain.Dtos.IngredientDtos;
using RestaurantAPI.Domain.Mapping;
using RestaurantAPI.Domain.Models.MenuRelated;
using RestaurantAPI.Domain.ServicesAbstractions;
using RestaurantAPI.Exceptions;

namespace Core.Services
{
    public class IngredientsService : IIngredientsService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IDataLogger logger;

        public IngredientsService(IUnitOfWork unitOfWork, IDataLogger logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Create(CreateOrUpdateIngredient ingredient)
        {
            if (ingredient == null)
            {
                logger.LogError($"Null argument from controller: {nameof(ingredient)}");
                throw new ArgumentNullException(nameof(ingredient));
            }

            Ingredient ingredientData = IngredientMapping.MapToIngredient(ingredient);

            await _unitOfWork.IngredientRepository.AddAsync(ingredientData);

            bool result = await _unitOfWork.SaveChangesAsync();
            logger.LogInfo($"Ingredient with id {ingredientData.Id} added");

            return result;
        }

        public async Task<bool> Delete(int ingredientId)
        {
            try
            {
                await _unitOfWork.IngredientRepository.DeleteAsync(ingredientId);
            }
            catch (EntityNotFoundException exception)
            {
                logger.LogError(exception.Message, exception);

                return false;
            }

            bool response = await _unitOfWork.SaveChangesAsync();
            logger.LogInfo($"Ingredient with id {ingredientId} deleted");

            return response;
        }

        public async Task<IEnumerable<Ingredient>> GetAll()
        {
            var ingredientsFromDb = await _unitOfWork.IngredientRepository.GetAllAsync();

            return ingredientsFromDb;
        }

        public async Task<Ingredient> GetById(int ingredientId)
        {
            var ingredientFromDb = await _unitOfWork.IngredientRepository.GetByIdAsync(ingredientId);

            return ingredientFromDb;
        }

        public async Task<bool> Update(int ingredientId, CreateOrUpdateIngredient ingredient)
        {
            if (ingredient == null)
            {
                logger.LogError($"Null argument from controller: {nameof(ingredient)}");

                throw new ArgumentNullException(nameof(ingredient));
            }

            Ingredient ingredientData = IngredientMapping.MapToIngredient(ingredient);

            try
            {
                await _unitOfWork.IngredientRepository.UpdateAsync(ingredientId, ingredientData);
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
            logger.LogInfo($"Ingredient with id {ingredientId} updated");

            return response;
        }
    }
}
