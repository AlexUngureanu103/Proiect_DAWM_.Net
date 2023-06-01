using RestaurantAPI.Domain.Models.Users;

namespace RestaurantAPI.Domain
{
    public interface IAuthorizationService
    {
        string GetToken(User user, string role);

        bool ValidateToken(string tokenString);

        string HashPassword(string password);

        bool VerifyHashedPassword(string hashedPassword, string password);
    }
}
