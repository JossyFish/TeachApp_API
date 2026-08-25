using Auth.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuthDBContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString(nameof(AuthDBContext)));
            });


            //services.AddScoped<IUsersRepository, UsersRepository>();
            //services.AddScoped<ICacheUsersRepository, CacheUsersRepository>();
            //services.AddScoped<IEmailTemplates, EmailTemplates>();
            //services.AddScoped<IEmailSender, EmailSender>();
            //services.AddScoped<IEmailService, EmailService>();

            //services.AddScoped<DbInitializer>();

            return services;
        }

    }
}
