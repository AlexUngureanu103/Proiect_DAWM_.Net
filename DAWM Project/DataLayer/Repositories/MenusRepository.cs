using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Domain.Models.MenuRelated;
using RestaurantAPI.Domain.RepositoriesAbstractions;
using RestaurantAPI.Exceptions;
using System.Linq;

namespace DataLayer.Repositories
{
    public class MenusRepository : RepositoryBase<Menu>, IMenusRepository
    {
        private readonly DbSet<Menu> _dbSet;
        public MenusRepository(AppDbContext dbContext) : base(dbContext)
        {
            dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = dbContext.Set<Menu>();
        }

        public new async Task UpdateAsync(int entityId, Menu entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var entityFromDb = await GetByIdAsync(entityId);
            if (entityFromDb == null)
                throw new EntityNotFoundException($"{nameof(Recipe)} with id {entity.Id} does not exist.");

            entityFromDb.Name = entity.Name;
            entityFromDb.Price = entity.Price;
        }
    }
}
