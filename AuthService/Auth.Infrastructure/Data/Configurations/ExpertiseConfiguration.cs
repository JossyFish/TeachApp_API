using Auth.Domain.Entites;
using Auth.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Data.Configurations
{
    public class ExpertiseConfiguration : IEntityTypeConfiguration<ExpertiseEntity>
    {
        public void Configure(EntityTypeBuilder<ExpertiseEntity> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(e => e.Name).IsUnique();

            var expertises = Enum
                .GetValues<Expertise>()
                .Select(e => new ExpertiseEntity
                {
                    Id = (int)e,
                    Name = e.ToString()
                });

            builder.HasData(expertises);
        }
    }
}
