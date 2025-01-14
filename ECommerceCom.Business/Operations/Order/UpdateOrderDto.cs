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
        public int OrderId { get; set; } // Hangi siparişin güncelleneceğini belirten zorunlu alan
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than zero.")]
        public decimal? TotalAmount { get; set; } // Toplam tutar zorunlu (nullable fakat geçerli olmalı)
        public ICollection<int> OrderProductIds { get; set; } = new List<int>(); // En az bir ürün ID'si zorunlu
        // Optional alanlar
        public int? CustomerId { get; set; } // Müşteri bilgisi opsiyonel
        // Quantity'yi OrderProduct için ekliyoruz
        public Dictionary<int, int> ProductQuantities { get; set; } = new Dictionary<int, int>(); // ProductId ve Quantity
    }
}
