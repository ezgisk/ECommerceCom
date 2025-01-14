using ECommerceCom.Business.Operations.Order.Dtos;
using ECommerceCom.WepApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ECommerceCom.WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _orderService.GetOrder(id);
            if (order is null) 
                return NotFound();
            else
                return Ok(order);
        }
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetOrders();
            return Ok(orders);

        }
        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrder(id);
            if (!result.IsSucceed)
                return NotFound(result.Message);
            else
                return Ok();

        }

        // POST api/Orders
        [HttpPost]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<IActionResult> AddOrder(AddOrderRequest request)
        {
            // Map AddOrderRequest to AddOrderDto
            var addOrderDto = new AddOrderDto
            {
                OrderDate = request.OrderDate,
                TotalAmount = request.TotalAmount,
                CustomerId = request.CustomerId
            };

            // Map OrderProducts from AddOrderRequest to AddOrderProductDto
            addOrderDto.OrderProducts = new List<AddOrderProductDto>();

            foreach (var product in request.OrderProducts)
            {
                var addOrderProductDto = new AddOrderProductDto
                {
                    ProductId = product.ProductId,
                    Quantity = product.Quantity
                };

                addOrderDto.OrderProducts.Add(addOrderProductDto);
            }

            // Call the service to add the order
            var result = await _orderService.AddOrder(addOrderDto);

            if (!result.IsSucceed)
            {
                return BadRequest(result.Message); // Return BadRequest if the order creation failed
            }

            return Ok(new { Message = "Order successfully created." }); // Return OK response if the order was successfully created
        }

        [HttpPatch("{id}/totalAmount")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdjustOrderTotalAmount(int id, decimal changeTo)
        {
            var result = await _orderService.AdjustOrderTotalAmount(id, changeTo);
            if (!result.IsSucceed)
                return NotFound();
            else
                return Ok();
        }

    }
}
