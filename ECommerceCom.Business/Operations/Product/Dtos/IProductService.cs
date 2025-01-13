using ECommerceCom.Business.Operations.Order.Dtos;
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
    }
}
