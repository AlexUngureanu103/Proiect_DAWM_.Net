using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Domain.Dtos.DishesTypeDtos
{
    public class CreateOrUpdateDishesType
    {
        [Required]
        [MaxLength(63)]
        public string Name { get; set; }
    }
}
