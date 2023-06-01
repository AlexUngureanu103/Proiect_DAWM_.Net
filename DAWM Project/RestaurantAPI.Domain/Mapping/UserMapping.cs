using RestaurantAPI.Domain.Dtos;
using RestaurantAPI.Domain.Models.Users;

namespace RestaurantAPI.Domain.Mapping
{
    public static class UserMapping
    {
        public static User MapToUser(CreateOrUpdateUser user)
        {
            return new User
            {
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
                PersonalData = new PersonalData
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName
                }
            };
        }
    }
}
