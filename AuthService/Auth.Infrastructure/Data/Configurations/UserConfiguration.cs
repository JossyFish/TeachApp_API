using Auth.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(u => u.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.LastName)
                .HasColumnName("last_name")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("ix_users_email");

            builder.Property(u => u.PasswordHash)
                .HasColumnName("password_hash")
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.CardLastDigits)
                .HasColumnName("card_last_digits")
                .HasMaxLength(4);

            builder.Property(u => u.CardBrand)
                .HasColumnName("card_brand")
                .HasMaxLength(20);

            builder.Property(u => u.IsActive)
                .HasColumnName("is_active")
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(u => u.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(u => u.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(u => u.LastLogin)
                .HasColumnName("last_login")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "user_roles",
                    j => j.HasOne<RoleEntity>().WithMany().HasForeignKey("role_id"),
                    j => j.HasOne<UserEntity>().WithMany().HasForeignKey("user_id")
                );

            builder.HasIndex(u => u.IsActive)
                .HasDatabaseName("ix_users_is_active");

            builder.HasIndex(u => new { u.Email, u.IsActive })
                .HasDatabaseName("ix_users_email_is_active");
        }
    }
}
