using RestaurantAPI.Domain.RepositoriesAbstractions;

namespace RestaurantAPI.Domain
{
    public interface IUnitOfWork
    {
        public IUserRepository UsersRepository { get; }

        public IIngredientRepository IngredientRepository { get; }

        Task<bool> SaveChangesAsync();
    }
}
