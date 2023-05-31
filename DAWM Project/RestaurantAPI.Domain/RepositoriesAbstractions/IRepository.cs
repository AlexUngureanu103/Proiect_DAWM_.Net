namespace RestaurantAPI.Domain.RepositoriesAbstractions
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);
        
        Task<T> AddAsync(T entity);
        
        Task<T> UpdateAsync(T entity);
        
        Task<T> DeleteAsync(int id);
    }
}
