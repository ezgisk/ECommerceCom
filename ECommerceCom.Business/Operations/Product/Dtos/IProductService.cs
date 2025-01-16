using ECommerceCom.Business.Operations.Order;
using ECommerceCom.Business.Operations.Order.Dtos;
using ECommerceCom.Business.Operations.Product.Dtos;
using ECommerceCom.Business.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceCom.Business.Operations.Feautere.Dtos
{
    public interface IProductService
    {
        Task<ServiceMessage> AddProduct(AddProductDto product);
        Task<ProductDto> GetProduct(int id);
        Task<List<ProductDto>> GetAllProducts();
        Task<ServiceMessage> AdjustProductStockQuantity(int id, int changeTo);
        Task<ServiceMessage> DeleteProduct(int id);
        Task<ServiceMessage> UpdateProduct(UpdateProductDto product);
    }
}
