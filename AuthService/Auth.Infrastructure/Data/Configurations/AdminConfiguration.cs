using Auth.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Data.Configurations
{
    public class AdminConfiguration : IEntityTypeConfiguration<AdminEntity>
    {
        public void Configure(EntityTypeBuilder<AdminEntity> builder)
        {
            builder.ToTable("admin_profiles");

            builder.HasKey(a => a.UserId);

            builder.Property(a => a.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(a => a.LastActionAt)
                .HasColumnName("last_action_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(a => a.User)
                .WithOne(u => u.Admin)
                .HasForeignKey<AdminEntity>(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(a => a.UserId)
                .IsUnique()
                .HasDatabaseName("ix_admin_profiles_user_id");
        }
    }
}
