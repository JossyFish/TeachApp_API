using Auth.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Data.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<StudentEntity>
    {
        public void Configure(EntityTypeBuilder<StudentEntity> builder)
        {
            builder.HasKey(s => s.UserId);

            builder.Property(s => s.UserId)
                .IsRequired();

            builder.Property(s => s.EnrolledCoursesCount)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(s => s.CompletedCoursesCount)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(s => s.LearningHours)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(s => s.CertificatesCount)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(s => s.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(s => s.StreakDays)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(s => s.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(s => s.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(s => s.User)
                .WithOne(u => u.Student)
                .HasForeignKey<StudentEntity>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(s => s.UserId)
                .IsUnique();

            builder.HasIndex(s => new { s.UserId, s.IsActive });
        }
    }
}
