using Auth.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Data.Configurations
{
    public class TeacherExpertiseConfiguration : IEntityTypeConfiguration<TeacherExpertiseEntity>
    {
        public void Configure(EntityTypeBuilder<TeacherExpertiseEntity> builder)
        {
            builder.HasKey(te => new { te.TeacherId, te.ExpertiseId });

            builder.HasOne(te => te.Teacher)
                .WithMany(t => t.Expertises)
                .HasForeignKey(te => te.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(te => te.Expertise)
                .WithMany(e => e.Teachers)
                .HasForeignKey(te => te.ExpertiseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(te => te.ExpertiseId);  
        }
    }
}
