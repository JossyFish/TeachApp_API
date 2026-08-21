using Auth.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Data.Configurations
{
    public class TeacherConfiguration : IEntityTypeConfiguration<TeacherEntity>
    {
        public void Configure(EntityTypeBuilder<TeacherEntity> builder)
        {
            builder.ToTable("teacher_profiles");

            builder.HasKey(t => t.UserId);

            builder.Property(t => t.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(t => t.Expertise)
                .HasColumnName("expertise")
                .HasMaxLength(255);

            builder.Property(t => t.Experience)
                .HasColumnName("experience")
                .HasMaxLength(50);

            builder.Property(t => t.Bio)
                .HasColumnName("bio")
                .HasMaxLength(1000);

            builder.Property(t => t.SubscriptionPlan)
                .HasColumnName("subscription_plan")
                .HasMaxLength(20)
                .HasDefaultValue("monthly");

            builder.Property(t => t.SubscriptionExpiresAt)
                .HasColumnName("subscription_expires_at")
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP + INTERVAL '1 month'");

            builder.Property(t => t.CardLastDigits)
                .HasColumnName("card_last_digits")
                .HasMaxLength(4);

            builder.Property(t => t.IsActive)
                .HasColumnName("is_active")
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(t => t.TotalStudents)
                .HasColumnName("total_students")
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(t => t.TotalCourses)
                .HasColumnName("total_courses")
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(t => t.AvgRating)
                .HasColumnName("avg_rating")
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(t => t.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(t => t.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(t => t.User)
                .WithOne(u => u.Teacher)
                .HasForeignKey<TeacherEntity>(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(t => t.UserId)
                .IsUnique()
                .HasDatabaseName("ix_teacher_profiles_user_id");

            builder.HasIndex(t => t.SubscriptionPlan)
                .HasDatabaseName("ix_teacher_profiles_subscription_plan");

            builder.HasIndex(t => t.IsActive)
                .HasDatabaseName("ix_teacher_profiles_is_active");

            builder.HasIndex(t => new { t.SubscriptionPlan, t.SubscriptionExpiresAt })
                .HasDatabaseName("ix_teacher_profiles_plan_expires");
        }
    }
}
