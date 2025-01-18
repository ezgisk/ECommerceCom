using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceCom.Business.Operations.Order.Dtos
{
    public class AddOrderProductDto
    {
        public int ProductId { get; set; } // The ID of the ordered product
        public int Quantity { get; set; } 
    }
}
