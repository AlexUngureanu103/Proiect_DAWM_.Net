using RestaurantAPI.Domain.RepositoriesAbstractions;

namespace RestaurantAPI.Domain
{
    public interface IUnitOfWork
    {
        public IUserRepository UsersRepository { get; }

        Task<bool> SaveChangesAsync();
    }
}
