using RestaurantAPI.Domain.Enums;

namespace RestaurantAPI.Domain.Dtos
{
    public class CreateOrUpdateUser
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }
    }
}
