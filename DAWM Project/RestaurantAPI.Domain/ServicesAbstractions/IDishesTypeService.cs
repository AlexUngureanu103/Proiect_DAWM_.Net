using RestaurantAPI.Domain.Dtos.DishesTypeDtos;
using RestaurantAPI.Domain.Models.MenuRelated;

namespace RestaurantAPI.Domain.ServicesAbstractions
{
    public interface IDishesTypeService
    {
        Task<bool> AddDishesType(CreateOrUpdateDishesType dishesType);

        Task<bool> DeleteDishesType(int dishesTypeId);

        Task<bool> UpdateDishesType(int dishesTypeId, CreateOrUpdateDishesType dishesType);

        Task<DishesType> GetDishesType(int dishesTypeId);

        Task<IEnumerable<DishesType>> GetAllDishesTypes();
    }
}
