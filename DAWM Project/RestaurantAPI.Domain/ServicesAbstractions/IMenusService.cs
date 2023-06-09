using RestaurantAPI.Domain.Dtos;
using RestaurantAPI.Domain.Dtos.DishesTypeDtos;
using RestaurantAPI.Domain.Models.MenuRelated;

namespace RestaurantAPI.Domain.ServicesAbstractions
{
    public interface IMenusService
    {
        Task<bool> AddMenu(CreateOrUpdateMenu menu);

        Task<bool> DeleteMenu(int menuId);

        Task<bool> UpateMenu(int menuId, CreateOrUpdateMenu menu);

        Task<Menu> GetMenuById(int menu);

        Task<IEnumerable<Menu>> GetAllMenus();
    }
}
