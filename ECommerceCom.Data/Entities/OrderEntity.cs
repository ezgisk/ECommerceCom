using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace ECommerceCom.Data.Entities
{
    public class OrderEntity : BaseEntity
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int CustomerId { get; set; } // Foreign Key
        public UserEntity Customer { get; set; } // Navigation Property
        public ICollection<OrderProductEntity> OrderProducts { get; set; } = new List<OrderProductEntity>();
    }

    public class OrderConfiguration : BaseConfiguration<OrderEntity>
    {
        public override void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.Property(x => x.OrderDate)
                .IsRequired();

            builder.Property(x => x.CustomerId)
                .IsRequired();

            builder.Property(x => x.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            // Configuring the relationship with Customer
            builder.HasOne(x => x.Customer) // Order has one Customer
                .WithMany() // Customer can have many Orders
                .HasForeignKey(x => x.CustomerId) // Foreign Key
                .OnDelete(DeleteBehavior.Cascade); // Deletion behavior (optional)

            // Configuring the relationship with OrderProducts
            builder.HasMany(x => x.OrderProducts) // Order can have many OrderProducts
                .WithOne(x => x.Order) // OrderProduct has one Order
                .HasForeignKey(x => x.OrderId); // Foreign Key

            // Call to base.Configure for all common configurations
            base.Configure(builder);
        }
    }
}
