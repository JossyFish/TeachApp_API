using Auth.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Data.Configurations
{
    public class TeacherConfiguration : IEntityTypeConfiguration<TeacherEntity>
    {
        public void Configure(EntityTypeBuilder<TeacherEntity> builder)
        {
            builder.HasKey(t => t.UserId);

            builder.Property(t => t.UserId)
                .IsRequired();

            builder.Property(t => t.Expertise)
                .HasMaxLength(255);

            builder.Property(t => t.Experience)
                .HasMaxLength(50);

            builder.Property(t => t.Bio)
                .HasMaxLength(1000);

            builder.Property(t => t.SubscriptionPlan)
                .HasMaxLength(20)
                .HasDefaultValue("monthly");

            builder.Property(t => t.SubscriptionExpiresAt)
                .IsRequired()
                .HasDefaultValueSql("DATEADD(MONTH, 1, GETDATE())");

            builder.Property(t => t.CardLastDigits)
                .HasMaxLength(4);

            builder.Property(t => t.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(t => t.TotalStudents)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(t => t.TotalCourses)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(t => t.AvgRating)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(t => t.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(t => t.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(t => t.User)
                .WithOne(u => u.Teacher)
                .HasForeignKey<TeacherEntity>(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(t => t.UserId)
                .IsUnique();

            builder.HasIndex(t => t.SubscriptionPlan);

            builder.HasIndex(t => t.IsActive);

            builder.HasIndex(t => new { t.SubscriptionPlan, t.SubscriptionExpiresAt });
        }
    }
}
