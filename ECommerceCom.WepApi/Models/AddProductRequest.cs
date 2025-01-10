using ECommerceCom.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace ECommerceCom.WepApi.Models
{
    public class AddProductRequest
    {
        [Required]
        public required string ProductName { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Price must have up to two decimal places.")]
        public decimal Price { get; set; }
        [Required]
        public int StockQuantity { get; set; }
    }
}
