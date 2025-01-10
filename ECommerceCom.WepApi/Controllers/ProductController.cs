using ECommerceCom.Business.Operations.Feautere.Dtos;
using ECommerceCom.Business.Operations.Order.Dtos;
using ECommerceCom.WepApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceCom.WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductRequest request)
        {
            // AddProductRequest'i AddProductDto'ya dönüştürme
            var addProductDto = new AddProductDto
            {
                Price = request.Price,
                ProductName = request.ProductName,
                StockQuantity = request.StockQuantity
            };

            // Asenkron işlem çağrısı
            var result = await _productService.AddProduct(addProductDto);

            if(result.IsSucceed)
                return Ok();
            else
                return BadRequest(result.Message);
        }
    }

}