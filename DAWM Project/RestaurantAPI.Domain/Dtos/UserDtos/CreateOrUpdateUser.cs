using RestaurantAPI.Domain.Enums;

namespace RestaurantAPI.Domain.Dtos.UserDtos
{
    public class CreateOrUpdateUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }
    }
}
