using RestaurantAPI.Domain.Enums;

namespace RestaurantAPI.Domain.Models.Users
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public Role Role { get; set; }
    }
}
