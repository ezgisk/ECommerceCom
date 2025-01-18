using ECommerceCom.Data.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ECommerceCom.Data.Entities
{
    public class UserEntity : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty; 
        public string LastName { get; set; } = string.Empty;  
        public string Email { get; set; } = string.Empty;     
        public string? PhoneNumber { get; set; }             
        public string Password { get; set; } = string.Empty; 
        public UserRole Role { get; set; }
        public ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();
    }

    public class UserConfiguration : BaseConfiguration<UserEntity>
    {
        public override void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            // FirstName alanı
            builder.Property(x => x.FirstName)
                   .IsRequired()
                   .HasMaxLength(50);

            // LastName alanı
            builder.Property(x => x.LastName)
                   .IsRequired()
                   .HasMaxLength(50);

            // Email alanı
            builder.Property(x => x.Email)
                   .IsRequired()
                   .HasMaxLength(100);

            // Email alanına benzersiz bir indeks ekleniyor
            builder.HasIndex(x => x.Email)
                   .IsUnique();

            // PhoneNumber alanı
            builder.Property(x => x.PhoneNumber)
                   .HasMaxLength(15);

            // Password alanı
            builder.Property(x => x.Password)
                   .IsRequired();

            // Orders ile ilişki tanımlanıyor
            builder.HasMany(x => x.Orders)
                   .WithOne(o => o.Customer)
                   .HasForeignKey(o => o.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Base sınıf konfigürasyonu
            base.Configure(builder);
        }
    }
}
