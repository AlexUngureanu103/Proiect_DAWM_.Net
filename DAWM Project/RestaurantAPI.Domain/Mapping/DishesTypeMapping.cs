using RestaurantAPI.Domain.Dtos.DishesTypeDtos;
using RestaurantAPI.Domain.Dtos.IngredientDtos;
using RestaurantAPI.Domain.Models.MenuRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
