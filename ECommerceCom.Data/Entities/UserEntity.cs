using ECommerceCom.Data.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceCom.Data.Entities
{
    public class UserEntity : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty; 
        public string LastName { get; set; } = string.Empty; 
        public string Email { get; set; } = string.Empty; 
        public string PhoneNumber { get; set; } = string.Empty; 
        public string Password { get; set; } = string.Empty; 
        public UserRole Role { get; set; }
        public ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();
    }
    public class UserConfiguration : BaseConfiguration<UserEntity>
    {
        public override void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.Property(x => x.FirstName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.LastName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.Email)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasIndex(x => x.Email)
                   .IsUnique(); 

 
            builder.Property(x => x.PhoneNumber)
                   .HasMaxLength(15);


            builder.Property(x => x.Password)
                   .IsRequired();


            builder.Property(x => x.Role)
                   .IsRequired();

        
            builder.HasMany(x => x.Orders)
                   .WithOne(o => o.Customer)
                   .HasForeignKey(o => o.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade); 

            base.Configure(builder);
        }
    }

}
