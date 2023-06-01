using RestaurantAPI.Domain.Dtos;

namespace RestaurantAPI.Domain.ServicesAbstractions
{
    public interface IUsersService
    {
        Task<bool> Register(CreateOrUpdateUser registerData);

        Task<bool> DeleteAccount(int id);
    }
}
