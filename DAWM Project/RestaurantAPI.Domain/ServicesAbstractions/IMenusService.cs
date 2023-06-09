using RestaurantAPI.Domain.Dtos.DishesTypeDtos;
using RestaurantAPI.Domain.Dtos.MenuDtos;
using RestaurantAPI.Domain.Models.MenuRelated;

namespace RestaurantAPI.Domain.ServicesAbstractions
{
    public interface IMenusService
    {
        Task<bool> AddMenu(CreateOrUpdateMenu menu);

        Task<bool> DeleteMenu(int menuId);

        Task<bool> UpateMenu(int menuId, CreateOrUpdateMenu menu);

        Task<MenuInfos> GetMenuById(int menu);

        Task<IEnumerable<MenuInfos>> GetAllMenus();

        Task<bool> AddMenuItem(int menuId, int recipieId);

        Task<bool> DeleteMenuItem(int menuId, int recipieId);
    }
}
