using System.ComponentModel.DataAnnotations;

namespace ECommerceCom.WepApi.Models
{
    public class UpdateOrderRequest
    {
        [Required(ErrorMessage = "Order ID is required.")]
        public int OrderId { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than zero.")]
        public decimal? TotalAmount { get; set; } 
        public ICollection<int> OrderProductIds { get; set; } = new List<int>();
        public int? CustomerId { get; set; } 
    }
}
