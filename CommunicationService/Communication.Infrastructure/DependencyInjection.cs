using Communication.Domain.Interfaces;
using Communication.Domain.Models.Options;
using Communication.Infrastructure.Consumers;
using Communication.Infrastructure.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Communication.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<AuthDBContext>(options =>
            //{
            //    options.UseSqlServer(configuration.GetConnectionString(nameof(AuthDBContext)));
            //});

            services.AddScoped<IEmailSender, EmailSender>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<EmailConsumer>();

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

                    cfg.ReceiveEndpoint(rabbitMQOptions.EmailQueueName, e =>
                    {
                        e.ConfigureConsumer<EmailConsumer>(context);
                    });

                    cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(1)));
                    cfg.ConfigureEndpoints(context);
                });
            });



            return services;
        }
    }
}
