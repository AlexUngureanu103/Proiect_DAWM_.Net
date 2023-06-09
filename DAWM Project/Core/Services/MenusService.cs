using RestaurantAPI.Domain;
using RestaurantAPI.Domain.Dtos;
using RestaurantAPI.Domain.Mapping;
using RestaurantAPI.Domain.Models.MenuRelated;
using RestaurantAPI.Domain.ServicesAbstractions;
using RestaurantAPI.Exceptions;

namespace Core.Services
{
    public class MenusService : IMenusService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IDataLogger logger;

        public MenusService(IUnitOfWork unitOfWork, IDataLogger logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> AddMenu(CreateOrUpdateMenu menu)
        {
            if (menu == null)
                throw new ArgumentNullException(nameof(menu));

            Menu menuData = MenuMapping.MapToMenu(menu);

            if (menuData == null)
                return false;

            await _unitOfWork.MenusRepository.AddAsync(menuData);

            bool result = await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<bool> DeleteMenu(int menuId)
        {
            try
            {
                await _unitOfWork.MenusRepository.DeleteAsync(menuId);
            }
            catch (EntityNotFoundException exception)
            {
                logger.LogError(exception.Message, exception);

                return false;
            }

            bool response = await _unitOfWork.SaveChangesAsync();

            return response;
        }

        public async Task<IEnumerable<Menu>> GetAllMenus()
        {
            var menusFromDb = await _unitOfWork.MenusRepository.GetAllAsync();

            return menusFromDb;
        }

        public async Task<Menu> GetMenuById(int menuId)
        {
            var menuFromDb = await _unitOfWork.MenusRepository.GetByIdAsync(menuId);

            return menuFromDb;
        }

        public async Task<bool> UpateMenu(int menuId, CreateOrUpdateMenu menu)
        {
            if (menu == null)
            {
                logger.LogError($"Null argument from controller: {nameof(menu)}");

                return false;
            }

            Menu menuData = MenuMapping.MapToMenu(menu);

            try
            {
                await _unitOfWork.MenusRepository.UpdateAsync(menuId, menuData);
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

            return response;
        }
    }
}
