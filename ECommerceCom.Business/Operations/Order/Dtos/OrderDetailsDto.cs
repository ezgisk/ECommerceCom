using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceCom.Business.Operations.Order.Dtos
{
    public class OrderDetailsDto
    {
        public int Id { get; set; }               
        public DateTime OrderDate { get; set; }   
        public decimal TotalAmount { get; set; }  
        public int CustomerId { get; set; }       // ID of the customer who placed the order
        public string CustomerName { get; set; }  
        public List<OrderProductDto> OrderProducts { get; set; } = new List<OrderProductDto>(); // List of products in the order
    }
}
