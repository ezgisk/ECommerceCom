using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceCom.Business.Operations.Order.Dtos
{
    public class AddOrderDto
    {
        public int OrderId { get; set; } 
        public DateTime OrderDate { get; set; } 
        public decimal TotalAmount { get; set; } 
        public int CustomerId { get; set; } // Customer ID associated with the order
        public List<AddOrderProductDto> OrderProducts { get; set; } = new List<AddOrderProductDto>(); // List of products in the order
    }
}
