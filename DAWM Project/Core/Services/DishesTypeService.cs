using RestaurantAPI.Domain;
using RestaurantAPI.Domain.Dtos.DishesTypeDtos;
using RestaurantAPI.Domain.Mapping;
using RestaurantAPI.Domain.Models.MenuRelated;
using RestaurantAPI.Domain.ServicesAbstractions;
using RestaurantAPI.Exceptions;

namespace Core.Services
{
    public class DishesTypeService : IDishesTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IDataLogger logger;

        public DishesTypeService(IUnitOfWork unitOfWork, IDataLogger logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Create(CreateOrUpdateDishesType dishesType)
        {
            if (dishesType == null)
            {
                logger.LogError($"Null argument from controller: {nameof(dishesType)}");
                throw new ArgumentNullException(nameof(dishesType));
            }

            DishesType dishesTypeData = DishesTypeMapping.MapToDishesType(dishesType);

            await _unitOfWork.DishesTypeRepository.AddAsync(dishesTypeData);

            bool result = await _unitOfWork.SaveChangesAsync();
            logger.LogInfo($"DishesType with id {dishesTypeData.Id} added");

            return result;
        }

        public async Task<bool> Delete(int dishesTypeId)
        {
            try
            {
                await _unitOfWork.DishesTypeRepository.DeleteAsync(dishesTypeId);
            }
            catch (EntityNotFoundException exception)
            {
                logger.LogError(exception.Message, exception);

                return false;
            }

            bool response = await _unitOfWork.SaveChangesAsync();
            logger.LogInfo($"DishesType with id {dishesTypeId} deleted");

            return response;
        }

        public async Task<IEnumerable<DishesType>> GetAll()
        {
            var dishesTypesFromDb = await _unitOfWork.DishesTypeRepository.GetAllAsync();

            return dishesTypesFromDb;
        }

        public async Task<DishesType> GetById(int dishesTypeId)
        {
            var dishesTypeFromDb = await _unitOfWork.DishesTypeRepository.GetByIdAsync(dishesTypeId);

            return dishesTypeFromDb;
        }

        public async Task<bool> Update(int dishesTypeId, CreateOrUpdateDishesType dishesType)
        {
            if (dishesType == null)
            {
                logger.LogError($"Null argument from controller: {nameof(dishesType)}");

                throw new ArgumentNullException(nameof(dishesType));
            }

            DishesType dishesTypeData = DishesTypeMapping.MapToDishesType(dishesType);
            try
            {
                await _unitOfWork.DishesTypeRepository.UpdateAsync(dishesTypeId, dishesTypeData);
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
            logger.LogInfo($"DishesType with id {dishesTypeId} updated");

            return response;
        }
    }
}
