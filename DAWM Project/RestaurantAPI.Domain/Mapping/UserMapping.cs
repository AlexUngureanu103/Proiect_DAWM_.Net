using RestaurantAPI.Domain.Dtos.UserDtos;
using RestaurantAPI.Domain.Models.Users;

namespace RestaurantAPI.Domain.Mapping
{
    public static class UserMapping
    {
        /// <summary>
        /// Creates User instance from CreateOrUpdateUser
        /// </summary>
        /// <param name="user">Create or Update user payload given from the client side </param>
        /// <returns></returns>
        public static User MapToUser(CreateOrUpdateUser user)
        {
            if (user == null)
                return null;
            return new User
            {
                Email = user.Email,
                PasswordHash = user.Password,
                Role = user.Role,
                FirstName = user.FirstName,
                LastName = user.LastName

            };
        }

        /// <summary>
        /// Creates User Public Data instance from User
        /// </summary>
        /// <param name="user">User for which we want to get the public data</param>
        /// <returns></returns>
        public static UserPublicData MapToUserPublicData(User user)
        {
            if (user == null)
                return null;

            return new UserPublicData
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }
    }
}
