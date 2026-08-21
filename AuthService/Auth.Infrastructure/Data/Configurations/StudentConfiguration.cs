using Auth.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Data.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<StudentEntity>
    {
        public void Configure(EntityTypeBuilder<StudentEntity> builder)
        {
            builder.ToTable("student_profiles");

            builder.HasKey(s => s.UserId);

            builder.Property(s => s.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(s => s.EnrolledCoursesCount)
                .HasColumnName("enrolled_courses_count")
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(s => s.CompletedCoursesCount)
                .HasColumnName("completed_courses_count")
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(s => s.LearningHours)
                .HasColumnName("learning_hours")
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(s => s.CertificatesCount)
                .HasColumnName("certificates_count")
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(s => s.StreakDays)
                .HasColumnName("streak_days")
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(s => s.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(s => s.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(s => s.User)
                .WithOne(u => u.Student)
                .HasForeignKey<StudentEntity>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(s => s.UserId)
                .IsUnique()
                .HasDatabaseName("ix_student_profiles_user_id");

            builder.HasIndex(s => new { s.UserId, s.IsActive })
                .HasDatabaseName("ix_student_profiles_user_id_is_active");
        }
    }
}
