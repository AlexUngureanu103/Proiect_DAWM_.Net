namespace RestaurantAPI.Domain.ServicesAbstractions
{
    public interface ICollectionService<T>
    {
        Task<IEnumerable<T>> GetAll();

        Task<T> Get(int id);

        Task<bool> Create(T model);

        Task<bool> Update(int id, T model);

        Task<bool> Delete(int id);
    }
}
