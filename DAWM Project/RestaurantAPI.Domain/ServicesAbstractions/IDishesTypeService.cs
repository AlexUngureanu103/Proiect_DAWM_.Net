using RestaurantAPI.Domain.Dtos.DishesTypeDtos;
using RestaurantAPI.Domain.Models.MenuRelated;

namespace RestaurantAPI.Domain.ServicesAbstractions
{
    public interface IDishesTypeService
    {
        Task<bool> Create(CreateOrUpdateDishesType dishesType);

        Task<bool> Delete(int dishesTypeId);

        Task<bool> Update(int dishesTypeId, CreateOrUpdateDishesType dishesType);

        Task<DishesType> GetById(int dishesTypeId);

        Task<IEnumerable<DishesType>> GetAll();
    }
}
