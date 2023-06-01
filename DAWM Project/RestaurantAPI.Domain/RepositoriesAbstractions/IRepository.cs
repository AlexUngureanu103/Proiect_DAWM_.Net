using RestaurantAPI.Domain.Models;

namespace RestaurantAPI.Domain.RepositoriesAbstractions
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);

        Task AddAsync(T entity);

        Task UpdateAsync(int entityId, T entity);

        Task DeleteAsync(int id);
    }
}
