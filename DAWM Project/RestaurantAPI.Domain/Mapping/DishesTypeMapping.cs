using RestaurantAPI.Domain.Dtos.DishesTypeDtos;
using RestaurantAPI.Domain.Models.MenuRelated;

namespace RestaurantAPI.Domain.Mapping
{
    public static class DishesTypeMapping
    {
        public static DishesType MapToDishesType(CreateOrUpdateDishesType dishesType)
        {
            if (dishesType == null)
                return null;

            return new DishesType
            {
                Name = dishesType.Name
            };
        }
    }
}
