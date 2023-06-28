using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Domain.Dtos.IngredientDtos
{
    public class CreateOrUpdateIngredient
    {
        [Required]
        [MaxLength(63)]
        public string Name { get; set; }
    }
}
