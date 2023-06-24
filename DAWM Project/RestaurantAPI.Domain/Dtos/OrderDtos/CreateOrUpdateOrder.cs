using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Domain.Dtos.OrderDtos
{
    public class CreateOrUpdateOrder
    {
        [Required]
        public int UserId { get; set; }
    }
}
