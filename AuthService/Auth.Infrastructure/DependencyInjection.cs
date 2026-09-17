using Auth.Domain.Interfaces;
using Auth.Domain.Mapping;
using Auth.Domain.Models.Options;
using Auth.Infrastructure.Data;
using Auth.Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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


            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<ICacheUsersRepository, CacheUsersRepository>();
            services.AddScoped<ISessionCacheRepository, SessionCacheRepository>();

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    var rabbitMQOptions = context.GetRequiredService<IOptions<RabbitMQOptions>>().Value;

                    cfg.Host(new Uri(rabbitMQOptions.ConnectionUrl), h =>
                    {
                        if (rabbitMQOptions.ConnectionUrl.StartsWith("amqps"))
                        {
                            h.UseSsl(s =>
                            {
                                s.Protocol = System.Security.Authentication.SslProtocols.Tls12;
                            });
                        }
                    });

                    cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(1)));
                    cfg.ConfigureEndpoints(context);
                });
            });



            services.AddMassTransitHostedService();

            return services;
        }

    }
}
