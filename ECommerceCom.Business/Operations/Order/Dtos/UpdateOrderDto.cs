using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceCom.Business.Operations.Order
{
    public class UpdateOrderDto
    {
        [Required]
        public int OrderId { get; set; } 
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than zero.")]
        public decimal? TotalAmount { get; set; } 
        public ICollection<int> OrderProductIds { get; set; } = new List<int>(); 
        // Optional 
        public int? CustomerId { get; set; } 
        public Dictionary<int, int> ProductQuantities { get; set; } = new Dictionary<int, int>(); 
    }
}
