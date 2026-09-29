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

            builder.Property(t => t.Experience)
                .HasMaxLength(50);

            builder.Property(t => t.Bio)
                .HasMaxLength(1000);

            builder.Property(t => t.SubscriptionPlanId)
                .IsRequired()
                .HasDefaultValue(1);  

            builder.HasOne(t => t.SubscriptionPlan)
                .WithMany(p => p.Teachers)
                .HasForeignKey(t => t.SubscriptionPlanId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(t => t.SubscriptionPlanId);

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

            builder.HasIndex(t => t.SubscriptionPlanId);
            builder.HasIndex(t => new { t.SubscriptionPlanId, t.SubscriptionExpiresAt });

            builder.HasIndex(t => t.IsActive);

        }
    }
}
