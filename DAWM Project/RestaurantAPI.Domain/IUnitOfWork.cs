namespace RestaurantAPI.Domain
{
    public interface IUnitOfWork
    {
        Task<bool> SaveChangesAsync();
    }
}
