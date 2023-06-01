using RestaurantAPI.Domain.Enums;

namespace RestaurantAPI.Domain.Models.Users
{
    public class User : BaseEntity
    {
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public Role Role { get; set; }

        public int PersonalDataId { get; set; }
        public PersonalData PersonalData { get; set; }
    }
}
