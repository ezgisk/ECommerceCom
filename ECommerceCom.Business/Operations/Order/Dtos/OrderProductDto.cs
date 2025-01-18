using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceCom.Business.Operations.Order.Dtos
{
    public class OrderProductDto
    {
        public int ProductId { get; set; }   
        public string ProductName { get; set; }  
        public int Quantity { get; set; }   // Quantity ordered
        public decimal UnitPrice { get; set; } 
    }
}
