using RestaurantAPI.Domain.Models.MenuRelated;
using RestaurantAPI.Domain.RepositoriesAbstractions;
using RestaurantAPI.Exceptions;

namespace DataLayer.Repositories
{
    public class DishesTypeRepository : RepositoryBase<DishesType>, IDishesTypeRepository
    {
        public DishesTypeRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public new async Task UpdateAsync(int entityId, DishesType entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var entityFromDb = await GetByIdAsync(entityId);
            if (entityFromDb == null)
                throw new EntityNotFoundException($"{nameof(DishesType)} with id {entity.Id} does not exist.");

            entityFromDb.Name = entity.Name;
        }
    }
}
