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

        [HttpGet]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _ordersService.GetAll();

            return Ok(orders);
        }

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
    }
}
