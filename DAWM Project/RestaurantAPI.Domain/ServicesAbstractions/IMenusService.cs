using RestaurantAPI.Domain.Dtos.MenuDtos;

namespace RestaurantAPI.Domain.ServicesAbstractions
{
    public interface IMenusService
    {
        Task<int> AddMenu(CreateOrUpdateMenu menu);

        Task<bool> DeleteMenu(int menuId);

        Task<bool> UpdateMenu(int menuId, CreateOrUpdateMenu menu);

        Task<MenuInfos> GetMenuById(int menu);

        Task<IEnumerable<MenuInfos>> GetAllMenus();

        Task<bool> AddMenuItem(int menuId, int recipieId);

        Task<bool> DeleteMenuItem(int menuId, int recipieId);
    }
}
