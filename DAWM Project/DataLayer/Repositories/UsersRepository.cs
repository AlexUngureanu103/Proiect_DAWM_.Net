using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Domain.Models.Users;
using RestaurantAPI.Domain.RepositoriesAbstractions;

namespace DataLayer.Repositories
{
    public class UsersRepository : IRepository<User>
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<User> _dbSet;

        public UsersRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<User>();
        }

        public async Task<User> AddAsync(User entity)
        {
            var addedEntity = await _dbSet.AddAsync(entity);

            return addedEntity.Entity;
        }

        public async Task<User> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                return null;

            _dbSet.Remove(entity);

            return entity;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<User> UpdateAsync(User entity)
        {
            await _dbContext.SaveChangesAsync();

            return entity;
        }
    }
}
