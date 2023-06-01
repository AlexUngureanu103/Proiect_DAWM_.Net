using RestaurantAPI.Domain.Models.Users;

namespace RestaurantAPI.Domain.RepositoriesAbstractions
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserByEmail(string email);
    }
}
