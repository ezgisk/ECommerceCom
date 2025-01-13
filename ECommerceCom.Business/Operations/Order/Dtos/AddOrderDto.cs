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
        public int OrderId { get; set; } // The created order's ID
        public DateTime OrderDate { get; set; } // Date the order was placed
        public decimal TotalAmount { get; set; } // Total amount for the order
        public int CustomerId { get; set; } // Customer ID associated with the order
        public List<AddOrderProductDto> OrderProducts { get; set; } = new List<AddOrderProductDto>(); // List of products in the order
    }
}
