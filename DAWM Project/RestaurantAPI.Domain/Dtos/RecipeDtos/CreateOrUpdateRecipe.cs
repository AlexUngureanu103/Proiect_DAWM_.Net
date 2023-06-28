using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Domain.Dtos.RecipeDtos
{
    public class CreateOrUpdateRecipe
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public float Price { get; set; }

        [Required]
        public int DishesTypeId { get; set; }

        [Required]
        public string ImageUrl { get; set; }


        [Required]
        public int PortionSize { get; set; }
    }
}
