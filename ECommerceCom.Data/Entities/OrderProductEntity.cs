using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ECommerceCom.Data.Entities
{
    public class OrderProductEntity : BaseEntity
    {
        public int OrderId { get; set; } // Foreign Key
        public OrderEntity Order { get; set; } // Navigation Property

        public int ProductId { get; set; } // Foreign Key
        public ProductEntity Product { get; set; } // Navigation Property

        public int Quantity { get; set; } // Ürün Adedi

        public class OrderProductConfiguration : IEntityTypeConfiguration<OrderProductEntity>
        {
            public void Configure(EntityTypeBuilder<OrderProductEntity> builder)
            {
                // Composite Key (OrderId ve ProductId)
                builder.HasKey(op => new { op.OrderId, op.ProductId });

                // OrderId ile ilişki tanımı
                builder.HasOne(op => op.Order)
                       .WithMany(o => o.OrderProducts)
                       .HasForeignKey(op => op.OrderId)
                       .OnDelete(DeleteBehavior.Cascade); // Silme davranışı

                // ProductId ile ilişki tanımı
                builder.HasOne(op => op.Product)
                       .WithMany()
                       .HasForeignKey(op => op.ProductId)
                       .OnDelete(DeleteBehavior.Restrict); // Silme davranışı

                // Quantity alanı için zorunlu yapılandırma
                builder.Property(op => op.Quantity)
                       .IsRequired();

                // Ek kurallar: ID'yi yoksay
                builder.Ignore(op => op.Id); // Eğer BaseEntity içinde Id varsa ve kullanılmıyorsa
            }
        }
    }
}
