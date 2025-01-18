using ECommerceCom.Business.Operations.Feautere.Dtos;
using ECommerceCom.Business.Operations.Order.Dtos;
using ECommerceCom.Business.Operations.Product.Dtos;
using ECommerceCom.WepApi.Filters;
using ECommerceCom.WepApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceCom.WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct(AddProductRequest request)
        {
            
            var addProductDto = new AddProductDto
            {
                Price = request.Price,
                ProductName = request.ProductName,
                StockQuantity = request.StockQuantity
            };

            var result = await _productService.AddProduct(addProductDto);

            if (result.IsSucceed)
                return Ok();
            else
                return BadRequest(result.Message);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProduct(id);
            if (product == null)
                return NotFound();
            else
                return Ok(product);

        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProducts();

            return Ok(products);
        }
        [HttpPatch("{id}/stockQuantity")]
        public async Task<IActionResult> AdjustProductStockQuantity(int id, int changeTo)
        {
            var result = await _productService.AdjustProductStockQuantity(id, changeTo);
            if (!result.IsSucceed)
                return NotFound();
            else
                return Ok();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProduct(id);
            if (!result.IsSucceed)
                return NotFound(result.Message);
            else
                return Ok();

        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        //[TimeControllerFilter]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto updateProductRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            var updateProductDto = new UpdateProductDto
            {
                ProductId = id,
                Price = updateProductRequest.Price,
                StockQuantity = updateProductRequest.StockQuantity,
                ProductName = updateProductRequest.ProductName,
            };

            var result = await _productService.UpdateProduct(updateProductDto);

            if (!result.IsSucceed)
            {
                return NotFound(result.Message);
            }

            return await GetProduct(id);
        }


    }

}