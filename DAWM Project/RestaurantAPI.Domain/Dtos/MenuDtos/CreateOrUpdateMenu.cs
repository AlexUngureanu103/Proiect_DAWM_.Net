using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Domain.Dtos.MenuDtos
{
    public class CreateOrUpdateMenu
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public float Price { get; set; }

        [Required]
        public string ImageUrl { get; set; }
    }
}
