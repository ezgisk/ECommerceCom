using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceCom.Business.Operations.Order.Dtos
{
    public class OrderDetailsDto
    {
        public int Id { get; set; }               // Order ID
        public DateTime OrderDate { get; set; }   // Date the order was placed
        public decimal TotalAmount { get; set; }  // Total amount of the order
        public int CustomerId { get; set; }       // ID of the customer who placed the order
        public string CustomerName { get; set; }  // Customer's name
        public List<OrderProductDto> OrderProducts { get; set; } = new List<OrderProductDto>(); // List of products in the order
    }
}
