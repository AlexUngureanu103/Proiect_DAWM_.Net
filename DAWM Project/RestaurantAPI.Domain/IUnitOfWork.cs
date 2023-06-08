using RestaurantAPI.Domain.RepositoriesAbstractions;

namespace RestaurantAPI.Domain
{
    public interface IUnitOfWork
    {
        public IUserRepository UsersRepository { get; }

        public IIngredientRepository IngredientRepository { get; }

        public IRecipeRepository RecipeRepository { get; }

        Task<bool> SaveChangesAsync();
    }
}
