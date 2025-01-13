using ECommerceCom.Business.Operations.Order.Dtos;
using ECommerceCom.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace ECommerceCom.WepApi.Models
{
    public class AddOrderRequest
    {
        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than zero.")]
        public decimal TotalAmount { get; set; }

        [Required]
        public int CustomerId { get; set; } // Foreign Key for Customer

        [Required]
        public List<AddOrderProductRequest> OrderProducts { get; set; } = new List<AddOrderProductRequest>(); // List of products in the order
    }
}

