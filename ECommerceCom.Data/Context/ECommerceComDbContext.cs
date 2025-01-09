using ECommerceCom.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using static ECommerceCom.Data.Entities.OrderProductEntity;

namespace ECommerceCom.Data.Context
{
    public class ECommerceComDbContext : DbContext
    {
        public ECommerceComDbContext(DbContextOptions<ECommerceComDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }

        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<OrderEntity> Orders => Set<OrderEntity>();
        public DbSet<ProductEntity> Products => Set<ProductEntity>();
        public DbSet<OrderProductEntity> OrderProducts => Set<OrderProductEntity>();

        public override async Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken = default)
        {
            // Get all entities that are either added or modified and are of type BaseEntity
            var entities = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                var entityType = entity.Entity as BaseEntity;
                if (entityType != null)
                {
                    // Ensure CreatedAt is in UTC
                    if (entityType.CreatedDate.Kind != DateTimeKind.Utc)
                    {
                        entityType.CreatedDate = entityType.CreatedDate.ToUniversalTime();
                    }

                    // Ensure UpdatedAt is in UTC
                    if (entityType.UpdatedDate.Kind != DateTimeKind.Utc)
                    {
                        entityType.UpdatedDate = entityType.UpdatedDate.ToUniversalTime();
                    }
                }
            }

            // Call the base SaveChangesAsync method to persist changes
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
