using Auth.Domain.Entites;
using Auth.Domain.Models.Options;
using Auth.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Auth.Infrastructure.Data
{
    public class AuthDBContext(DbContextOptions <AuthDBContext> options, IOptions<AuthorizationOptions> authOptions) : DbContext(options)
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<StudentEntity> StudentProfiles { get; set; }
        public DbSet<TeacherEntity> TeacherProfiles { get; set; }
        public DbSet<AdminEntity> AdminProfiles { get; set; }
        public DbSet<ExpertiseEntity> Expertises { get; set; }
        public DbSet<TeacherExpertiseEntity> TeacherExpertises { get; set; }
        public DbSet<SubscriptionPlanEntity> SubscriptionPlans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthDBContext).Assembly);
            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration(authOptions.Value));
        }

    }
}
