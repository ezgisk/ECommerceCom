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
    }
}
