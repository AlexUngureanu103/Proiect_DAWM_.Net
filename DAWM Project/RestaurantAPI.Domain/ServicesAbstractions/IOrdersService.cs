using RestaurantAPI.Domain.Dtos.OrderDtos;

namespace RestaurantAPI.Domain.ServicesAbstractions
{
    public interface IOrdersService
    {
        Task<bool> Create(CreateOrUpdateOrder order);

        Task<bool> Delete(int orderId);

        Task<bool> Update(int orderId, CreateOrUpdateOrder order);

        Task<OrderInfo> GetById(int orderId);

        Task<IEnumerable<OrderInfo>> GetAll(int userId);

        Task<bool> AddOrderItem(int orderId, int menuId);

        Task<bool> DeleteOrderItem(int orderId, int menuId);

        Task<bool> AddOrderSingleItem(int orderId, int recipeid);

        Task<bool> DeleteOrderSingleItem(int orderId, int recipeId);
    }
}
