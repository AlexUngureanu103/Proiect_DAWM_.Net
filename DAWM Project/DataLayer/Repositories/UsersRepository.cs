using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Domain.Models.Users;
using RestaurantAPI.Domain.RepositoriesAbstractions;
using RestaurantAPI.Exceptions;

namespace DataLayer.Repositories
{
    public class UsersRepository : RepositoryBase<User>, IUserRepository
    {
        public UsersRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await GetRecords().FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }

        public new async Task UpdateAsync(int entityId, User entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var entityFromDb = await GetByIdAsync(entityId);
            if (entityFromDb == null)
                throw new EntityNotFoundException($"{nameof(User)} with id {entity.Id} does not exist.");

            entityFromDb.PersonalData = entity.PersonalData;
            entityFromDb.PersonalDataId = entity.PersonalDataId;
            entityFromDb.Role = entity.Role;
            entityFromDb.Email = entity.Email;
            entityFromDb.PasswordHash = entity.PasswordHash;
        }
    }
}
