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

            // Customer ilişkisini yapılandırma
            builder.HasOne(x => x.Customer) // Order, bir Customer'a sahip
                .WithMany() // Customer, birden fazla Order'a sahip olabilir
                .HasForeignKey(x => x.CustomerId) // Foreign Key
                .OnDelete(DeleteBehavior.Cascade); // Silme davranışı (optional)

            // OrderProducts ilişkisini yapılandırma
            builder.HasMany(x => x.OrderProducts) // Order, birden fazla OrderProduct'a sahip
                .WithOne(x => x.Order) // OrderProduct, bir Order'a sahip
                .HasForeignKey(x => x.OrderId); // Foreign Key

            // base.Configure çağrısı, tüm ortak yapılandırmaların yapılması için
            base.Configure(builder);
        }
    }
}
