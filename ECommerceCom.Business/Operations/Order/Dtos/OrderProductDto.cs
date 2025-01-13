using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceCom.Business.Operations.Order.Dtos
{
    public class OrderProductDto
    {
        public int ProductId { get; set; }   // Product ID
        public string ProductName { get; set; }  // Product name (optional, you can also fetch this from the product repository)
        public int Quantity { get; set; }   // Quantity ordered
        public decimal UnitPrice { get; set; } // Price per unit of the product
    }
}
