using RestaurantAPI.Domain;
using RestaurantAPI.Domain.RepositoriesAbstractions;

namespace DataLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        public IUserRepository UsersRepository { get; }

        public IIngredientRepository IngredientRepository { get; }

        public IRecipeRepository RecipeRepository { get; }

        private readonly AppDbContext _dbContext;

        private readonly IDataLogger logger;

        public UnitOfWork
        (
            AppDbContext dbContext,
            IUserRepository usersRepository,
            IIngredientRepository ingredientRepository,
            IRecipeRepository recipeRepository,
            IDataLogger logger
        )
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            UsersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            IngredientRepository = ingredientRepository ?? throw new ArgumentNullException(nameof(ingredientRepository));
            RecipeRepository = recipeRepository ?? throw new ArgumentNullException(nameof(recipeRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception exception)
            {
                string errorMessage = "Error when saving to the database:\n ";

                logger.LogError(errorMessage, exception);

                return false;
            }
        }
    }
}
