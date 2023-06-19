using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Domain;
using RestaurantAPI.Domain.Dtos.OrderDtos;
using RestaurantAPI.Domain.ServicesAbstractions;

namespace DAWM_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;

        private readonly IDataLogger logger;

        public OrdersController(IOrdersService rdersService, IDataLogger logger)
        {
            this._ordersService = rdersService ?? throw new ArgumentNullException(nameof(rdersService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get all orders. Authentication required : User, Admin
        /// </summary>
        /// <returns>OkResult containing the orders</returns>
        [HttpGet]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _ordersService.GetAll();

            return Ok(orders);
        }

        /// <summary>
        /// Get an order by id. Authentication required : User, Admin
        /// </summary>
        /// <param name="orderId">Order id to get</param>
        /// <returns>OkResult if the get process was successful. Otherwise NotFoundResult</returns>
        [HttpGet]
        [Route("{orderId}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _ordersService.GetById(orderId);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        /// <summary>
        /// Add a new order. Authentication required : User, Admin
        /// </summary>
        /// <param name="payload">Order to add</param>
        /// <returns>OkResult if the create process was successful. Otherwise BadRequestResult</returns>
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> AddOrder(CreateOrUpdateOrder payload)
        {
            bool result = await _ordersService.Create(payload);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// Update an order. Authentication required : User, Admin
        /// </summary>
        /// <param name="orderId">Order id to be uodated</param>
        /// <param name="payload">New order data</param>
        /// <returns>OkResult if the update process was successful. Otherwise BadRequestResult</returns>
        [HttpPut]
        [Authorize(Roles = "User,Admin")]
        [Route("{orderId}")]
        public async Task<IActionResult> UpdateOrder(int orderId, CreateOrUpdateOrder payload)
        {
            bool result = await _ordersService.Update(orderId, payload);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// Delete an order. Authentication required : User, Admin
        /// </summary>
        /// <param name="orderId">Order Id to be deleted</param>
        /// <returns>OkResult if the delete process was successful. Otherwise BadRequestResult</returns>
        [HttpDelete]
        [Authorize(Roles = "User,Admin")]
        [Route("{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            bool result = await _ordersService.Delete(orderId);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// Add an item to an order. Authentication required : User, Admin
        /// </summary>
        /// <param name="orderId">Order id to add menu into</param>
        /// <param name="menuId">Menu id to add</param>
        /// <returns>OkResult if the create process was successful. Otherwise BadRequestResult</returns>
        [HttpPut]
        [Authorize(Roles = "User,Admin")]
        [Route("addItem/{orderId}/{menuId}")]
        public async Task<IActionResult> AddOrderItem(int orderId, int menuId)
        {
            bool result = await _ordersService.AddOrderItem(orderId, menuId);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// Delete an item from an order. Authentication required : User, Admin
        /// </summary>
        /// <param name="orderId">Order id to delete menu from</param>
        /// <param name="menuId">Menu id to delete(remove) </param>
        /// <returns>OkResult if the delete process was successful. Otherwise BadRequestResult</returns>
        [HttpPut]
        [Authorize(Roles = "User,Admin")]
        [Route("deleteItem/{orderId}/{menuId}")]
        public async Task<IActionResult> DeleteOrderItem(int orderId, int menuId)
        {
            bool result = await _ordersService.DeleteOrderItem(orderId, menuId);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// Add a single item to an order. Authentication required : User, Admin
        /// </summary>
        /// <param name="orderId">Order id to add the recipe</param>
        /// <param name="recipieId">Recipe id to add </param>
        /// <returns>OkResult if the create process was successful. Otherwise BadRequestResult</returns>
        [HttpPut]
        [Authorize(Roles = "User,Admin")]
        [Route("addSingleItem/{orderId}/{recipieId}")]
        public async Task<IActionResult> AddOrderSingleItem(int orderId, int recipieId)
        {
            bool result = await _ordersService.AddOrderSingleItem(orderId, recipieId);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// Delete a single item from an order. Authentication required : User, Admin
        /// </summary>
        /// <param name="orderId">Order id to delete recipe from</param>
        /// <param name="recipieId">Recipe id to delete(remove)</param>
        /// <returns>OkResult if the delete process was successful. Otherwise BadRequestResult</returns>
        [HttpPut]
        [Authorize(Roles = "User,Admin")]
        [Route("deleteSingleItem/{orderId}/{recipieId}")]
        public async Task<IActionResult> DeleteOrderSingleItem(int orderId, int recipieId)
        {
            bool result = await _ordersService.DeleteOrderSingleItem(orderId, recipieId);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
