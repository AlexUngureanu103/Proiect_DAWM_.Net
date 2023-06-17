using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Domain.Models.Orders;
using RestaurantAPI.Domain.RepositoriesAbstractions;
using RestaurantAPI.Exceptions;

namespace DataLayer.Repositories
{
    public class OrdersRepository : RepositoryBase<Order>, IOrdersRepository
    {
        public OrdersRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public new async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await GetRecords().Include(order => order.OrderItems).ToListAsync();
        }

        public new async Task<Order> GetByIdAsync(int entityId)
        {
            var resultFromDb = await GetRecords().Include(order => order.OrderItems).Where(order => order.Id == entityId).FirstOrDefaultAsync();

            return resultFromDb;
        }

        public new async Task UpdateAsync(int entityId, Order entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var entityFromDb = await GetByIdAsync(entityId);
            if (entityFromDb == null)
                throw new EntityNotFoundException($"{nameof(Order)} with id {entity.Id} does not exist.");

            entityFromDb.OrderDate = entity.OrderDate;
        }
    }
}
