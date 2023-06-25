using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Domain.Models.MenuRelated;
using RestaurantAPI.Domain.RepositoriesAbstractions;
using RestaurantAPI.Exceptions;
using System.Linq;

namespace DataLayer.Repositories
{
    public class MenusRepository : RepositoryBase<Menu>, IMenusRepository
    {
        public MenusRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public new async Task<IEnumerable<Menu>> GetAllAsync()
        {
            return await GetRecords()
                .Include(r => r.MenuItems)
                .ToListAsync();
        }

        public new async Task<Menu> GetByIdAsync(int entityId)
        {
            var resultFromDb = await GetRecords()
                .Include(r => r.MenuItems)
                .Where(r => r.Id == entityId)
                .FirstOrDefaultAsync();

            return resultFromDb;
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
