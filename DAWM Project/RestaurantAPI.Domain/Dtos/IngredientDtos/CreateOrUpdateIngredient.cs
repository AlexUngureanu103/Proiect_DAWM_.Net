using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Domain.Dtos.IngredientDtos
{
    public class CreateOrUpdateIngredient
    {
        [Required]
        [MaxLength(63)]
        public string Name { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total weight must be greater than 0.")]
        public double TotalWeight { get; set; }
    }
}
