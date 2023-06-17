using RestaurantAPI.Domain.RepositoriesAbstractions;

namespace RestaurantAPI.Domain
{
    public interface IUnitOfWork
    {
        public IUserRepository UsersRepository { get; }

        public IIngredientRepository IngredientRepository { get; }

        public IDishesTypeRepository DishesTypeRepository { get; }

        public IMenusRepository MenusRepository { get; }

        public IRecipeRepository RecipeRepository { get; }

        public IOrdersRepository OrdersRepository { get; }

        Task<bool> SaveChangesAsync();
    }
}
