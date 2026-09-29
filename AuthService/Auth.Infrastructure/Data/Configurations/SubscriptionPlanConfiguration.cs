using Auth.Domain.Entites;
using Auth.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Data.Configurations
{
    public class SubscriptionPlanConfiguration : IEntityTypeConfiguration<SubscriptionPlanEntity>
    {
        public void Configure(EntityTypeBuilder<SubscriptionPlanEntity> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.Code)
                .IsRequired()
                .HasConversion<int>();   

            builder.Property(p => p.Price)
                .HasPrecision(10, 2);

            builder.Property(p => p.Description)
                .HasMaxLength(500);

            builder.Property(p => p.IsActive)
                .HasDefaultValue(true);

            builder.HasIndex(p => p.Code).IsUnique();

            builder.HasData(
                new SubscriptionPlanEntity
                {
                    Id = 1,
                    Name = "Monthly",
                    Code = SubscriptionPlanCode.Monthly,
                    Price = 19.99m,
                    DurationDays = 30,
                    Description = "Оплата каждый месяц",
                    IsPopular = false,
                    IsActive = true
                },
                new SubscriptionPlanEntity
                {
                    Id = 2,
                    Name = "Yearly",
                    Code = SubscriptionPlanCode.Yearly,
                    Price = 179.99m,
                    DurationDays = 365,
                    Description = "Оплата раз в год. Экономия $60",
                    IsPopular = true,
                    IsActive = true
                },
                new SubscriptionPlanEntity
                {
                    Id = 3,
                    Name = "VIP",
                    Code = SubscriptionPlanCode.Vip,
                    Price = 39.99m,
                    DurationDays = 30,
                    Description = "Премиум доступ со всеми функциями",
                    IsPopular = false,
                    IsActive = true
                }
            );
        }
    }
}
