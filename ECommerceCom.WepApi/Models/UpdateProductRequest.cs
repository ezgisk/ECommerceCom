using System.ComponentModel.DataAnnotations;

namespace ECommerceCom.WepApi.Models
{
    public class UpdateProductRequest
    {
        [Required(ErrorMessage = "Product ID is required.")]
        public int ProductId { get; set; } // Unique identifier for the product
        [Required]
        public string ProductName { get; set; } // Name of the product
        [Required]
        public decimal Price { get; set; } // Price of the product
        [Required]
        public int StockQuantity { get; set; } // Available stock quantity for the product
    }
}
