using Auth.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.CardLastDigits)
                .HasMaxLength(4);

            builder.Property(u => u.CardBrand)
                .HasMaxLength(20);

            builder.Property(u => u.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(u => u.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(u => u.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(u => u.LastLogin)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasMany(u => u.Roles)
               .WithMany(r => r.Users)
               .UsingEntity<UserRoleEntity>(
               l => l.HasOne<RoleEntity>().WithMany().HasForeignKey(r => r.RoleId)
                   .OnDelete(DeleteBehavior.Cascade),
               r => r.HasOne<UserEntity>().WithMany().HasForeignKey(u => u.UserId)
                   .OnDelete(DeleteBehavior.Cascade));

            builder.HasIndex(u => u.IsActive);

            builder.HasIndex(u => new { u.Email, u.IsActive });
        }
    }
}
