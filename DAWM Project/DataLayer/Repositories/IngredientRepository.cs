using RestaurantAPI.Domain.Models.MenuRelated;
using RestaurantAPI.Domain.RepositoriesAbstractions;
using RestaurantAPI.Exceptions;

namespace DataLayer.Repositories
{
    public class IngredientRepository : RepositoryBase<Ingredient>, IIngredientRepository
    {
        public IngredientRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public new async Task UpdateAsync(int entityId, Ingredient entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var entityFromDb = await GetByIdAsync(entityId);
            if (entityFromDb == null)
                throw new EntityNotFoundException($"{nameof(Ingredient)} with id {entity.Id} does not exist.");

            entityFromDb.Name = entity.Name;
            entityFromDb.TotalWeight = entity.TotalWeight;
        }
    }
}
