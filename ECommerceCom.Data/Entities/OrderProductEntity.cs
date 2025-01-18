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

        public int Quantity { get; set; } // Product Quantity

        public class OrderProductConfiguration : IEntityTypeConfiguration<OrderProductEntity>
        {
            public void Configure(EntityTypeBuilder<OrderProductEntity> builder)
            {
                // Composite Key (OrderId and ProductId)
                builder.HasKey(op => new { op.OrderId, op.ProductId });

                // Defining the relationship with OrderId
                builder.HasOne(op => op.Order)
                       .WithMany(o => o.OrderProducts)
                       .HasForeignKey(op => op.OrderId)
                       .OnDelete(DeleteBehavior.Cascade); // Deletion behavior

                // Defining the relationship with ProductId
                builder.HasOne(op => op.Product)
                       .WithMany()
                       .HasForeignKey(op => op.ProductId)
                       .OnDelete(DeleteBehavior.Restrict); // Deletion behavior

                // Making the Quantity field required
                builder.Property(op => op.Quantity)
                       .IsRequired();

                // Additional rules: Ignore the Id if it's in BaseEntity and not needed
                builder.Ignore(op => op.Id); // If Id in BaseEntity is not used
            }
        }
    }
}
