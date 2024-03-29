﻿using RestaurantAPI.Domain;
using RestaurantAPI.Domain.Dtos.MenuDtos;
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
            {
                logger.LogError($"Null argument from controller: {nameof(menu)}");
                throw new ArgumentNullException(nameof(menu));
            }

            Menu menuData = MenuMapping.MapToMenu(menu);

            if (menuData == null)
                return false;

            await _unitOfWork.MenusRepository.AddAsync(menuData);

            bool result = await _unitOfWork.SaveChangesAsync();
            logger.LogInfo($"Menu with id {menuData.Id} added");

            return result;
        }

        public async Task<bool> AddMenuItem(int menuId, int recipeId)
        {
            Menu menu = await _unitOfWork.MenusRepository.GetByIdAsync(menuId);
            if (menu == null)
            {
                logger.LogWarn($"Menu with id {menuId} not found");
                return false;
            }

            if (await _unitOfWork.RecipeRepository.GetByIdAsync(recipeId) == null)
            {
                logger.LogWarn($"Recipe with id {recipeId} not found");
                return false;
            }

            menu.MenuItems.Add(new MenuItem { RecipeId = recipeId });

            bool response = await _unitOfWork.SaveChangesAsync();
            logger.LogInfo($"Menu with id {menuId} updated. Added Recipe with id {recipeId}");

            return response;
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
            logger.LogInfo($"Menu with id {menuId} deleted");

            return response;
        }

        public async Task<bool> DeleteMenuItem(int menuId, int recipeId)
        {
            Menu menu = await _unitOfWork.MenusRepository.GetByIdAsync(menuId);
            if (menu == null)
                if (menu == null)
                {
                    logger.LogWarn($"Menu with id {menuId} not found");
                    return false;
                }

            var record = menu.MenuItems.Where(item => item.RecipeId == recipeId).FirstOrDefault();
            if (record == null)
            {
                logger.LogWarn($"Menu with Id:{menuId} doesn't contains Recipe with id {recipeId}");
                return false;
            }

            menu.MenuItems.Remove(record);

            bool response = await _unitOfWork.SaveChangesAsync();
            logger.LogInfo($"Menu with id {menuId} updated. Removed Recipe with id {recipeId}");
            return response;
        }

        public async Task<IEnumerable<MenuInfos>> GetAllMenus()
        {
            var menusFromDb = await _unitOfWork.MenusRepository.GetAllAsync();

            return menusFromDb.Select(m => MenuMapping.MapToMenuInfos(m)).ToList();
        }

        public async Task<MenuInfos> GetMenuById(int menuId)
        {
            var menuFromDb = await _unitOfWork.MenusRepository.GetByIdAsync(menuId);

            return MenuMapping.MapToMenuInfos(menuFromDb);
        }

        public async Task<bool> UpdateMenu(int menuId, CreateOrUpdateMenu menu)
        {
            if (menu == null)
            {
                logger.LogError($"Null argument from controller: {nameof(menu)}");

                throw new ArgumentNullException(nameof(menu));
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
            logger.LogInfo($"Menu with id {menuId} updated");

            return response;
        }
    }
}
