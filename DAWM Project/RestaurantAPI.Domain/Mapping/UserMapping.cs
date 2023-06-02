using RestaurantAPI.Domain.Dtos.UserDtos;
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
                PasswordHash = user.Password,
                Role = user.Role,
                FirstName = user.FirstName,
                LastName = user.LastName

            };
        }
    }
}
