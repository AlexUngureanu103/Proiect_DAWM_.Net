using RestaurantAPI.Domain;
using RestaurantAPI.Domain.Dtos.OrderDtos;
using RestaurantAPI.Domain.Mapping;
using RestaurantAPI.Domain.Models.Orders;
using RestaurantAPI.Domain.ServicesAbstractions;
using RestaurantAPI.Exceptions;

namespace Core.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IDataLogger logger;

        public OrdersService(IUnitOfWork unitOfWork, IDataLogger logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<bool> Create(CreateOrUpdateOrder order)
        {
            if (order == null)
            {
                logger.LogError($"Null argument from controller: {nameof(order)}");
                throw new ArgumentNullException(nameof(order));
            }

            Order orderData = OrderMapping.MapToOrder(order);

            if (orderData == null)
                return false;

            await _unitOfWork.OrdersRepository.AddAsync(orderData);

            bool result = await _unitOfWork.SaveChangesAsync();
            logger.LogInfo($"Order with id {orderData.Id} added");

            return result;
        }

        public async Task<bool> AddOrderItem(int orderId, int menuId)
        {
            Order order = await _unitOfWork.OrdersRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                logger.LogWarn($"Order with id {orderId} not found");
                return false;
            }

            if (await _unitOfWork.MenusRepository.GetByIdAsync(menuId) == null)
            {
                logger.LogWarn($"Menu with id {menuId} not found");
                return false;
            }
            var orderItm = order.OrderItems.FirstOrDefault(x => x.MenuId == menuId);
            if (orderItm == null)
                order.OrderItems.Add(new OrderItems { MenuId = menuId, Quantity = 1 });
            else
            {
                orderItm.Quantity++;
            }

            bool response = await _unitOfWork.SaveChangesAsync();
            logger.LogInfo($"Order with id {orderId} updated. Added Menu with id {menuId}");

            return response;
        }

        public async Task<bool> Delete(int orderId)
        {
            try
            {
                await _unitOfWork.OrdersRepository.DeleteAsync(orderId);
            }
            catch (EntityNotFoundException exception)
            {
                logger.LogError(exception.Message, exception);

                return false;
            }

            bool response = await _unitOfWork.SaveChangesAsync();
            logger.LogInfo($"Order with id {orderId} deleted");

            return response;
        }

        public async Task<bool> DeleteOrderItem(int orderId, int menuId)
        {
            Order order = await _unitOfWork.OrdersRepository.GetByIdAsync(orderId);
            if (order == null)
                if (order == null)
                {
                    logger.LogWarn($"Order with id {orderId} not found");
                    return false;
                }

            var record = order.OrderItems.Where(item => item.MenuId == menuId).FirstOrDefault();
            if (record == null)
            {
                logger.LogWarn($"Order with Id:{orderId} doesn't contains Menu with id {menuId}");
                return false;
            }

            order.OrderItems.Remove(record);

            bool response = await _unitOfWork.SaveChangesAsync();
            logger.LogInfo($"Order with id {orderId} updated. Removed Menu with id {menuId}");
            return response;
        }

        public async Task<IEnumerable<OrderInfo>> GetAll(int userId)
        {
            var ordersFromDb = await _unitOfWork.OrdersRepository.GetAllAsync();

            foreach (var order in ordersFromDb)
            {
                foreach (var orderItem in order.OrderItems)
                {
                    orderItem.Menu = await _unitOfWork.MenusRepository.GetByIdAsync(orderItem.MenuId);
                }
            }

            return ordersFromDb
                .Where(order => order.UserId == userId)
                .Select(order => OrderMapping.MapToOrderInfos(order)).ToList();
        }

        public async Task<OrderInfo> GetById(int orderId)
        {
            var orderFromDb = await _unitOfWork.OrdersRepository.GetByIdAsync(orderId);

            if (orderFromDb != null && orderFromDb.OrderItems != null)
                foreach (var orderItem in orderFromDb.OrderItems)
                {
                    orderItem.Menu = await _unitOfWork.MenusRepository.GetByIdAsync(orderItem.MenuId);
                }

            return OrderMapping.MapToOrderInfos(orderFromDb);
        }

        public async Task<bool> Update(int orderId, CreateOrUpdateOrder order)
        {
            if (order == null)
            {
                logger.LogError($"Null argument from controller: {nameof(order)}");

                throw new ArgumentNullException(nameof(order));
            }

            Order orderData = OrderMapping.MapToOrder(order);

            try
            {
                await _unitOfWork.OrdersRepository.UpdateAsync(orderId, orderData);
            }
            catch (EntityNotFoundException exception)
            {
                logger.LogError(exception.Message, exception);

                return false;
            }
            catch (ArgumentNullException exception)
            {
                logger.LogError(exception.Message, exception);
                return false;
            }

            bool response = await _unitOfWork.SaveChangesAsync();
            logger.LogInfo($"Order with id {orderId} updated");

            return response;
        }

        public async Task<bool> AddOrderSingleItem(int orderId, int recipeId)
        {
            Order order = await _unitOfWork.OrdersRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                logger.LogWarn($"Order with id {orderId} not found");
                return false;
            }

            if (await _unitOfWork.RecipeRepository.GetByIdAsync(recipeId) == null)
            {
                logger.LogWarn($"Recipe with id {recipeId} not found");
                return false;
            }
            var orderSingleItm = order.OrderSingleItems.FirstOrDefault(x => x.RecipieId == recipeId);
            if (orderSingleItm == null)
                order.OrderSingleItems.Add(new OrderSingleItems { RecipieId = recipeId, Quantity = 1 });
            else
            {
                orderSingleItm.Quantity++;
            }

            bool response = await _unitOfWork.SaveChangesAsync();
            logger.LogInfo($"Order with id {orderId} updated. Added recipe with id {recipeId}");

            return response;
        }

        public async Task<bool> DeleteOrderSingleItem(int orderId, int recipeId)
        {
            Order order = await _unitOfWork.OrdersRepository.GetByIdAsync(orderId);
            if (order == null)
                if (order == null)
                {
                    logger.LogWarn($"Order with id {orderId} not found");
                    return false;
                }

            var record = order.OrderSingleItems.Where(item => item.RecipieId == recipeId).FirstOrDefault();
            if (record == null)
            {
                logger.LogWarn($"Order with Id:{orderId} doesn't contains Recipe with id {recipeId}");
                return false;
            }

            order.OrderSingleItems.Remove(record);

            bool response = await _unitOfWork.SaveChangesAsync();
            logger.LogInfo($"Order with id {orderId} updated. Removed Recipe with id {recipeId}");
            return response;
        }
    }
}
