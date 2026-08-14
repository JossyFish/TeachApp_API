
using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;

namespace Teach.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //builder.AddServiceDefaults();


            builder.Services.AddSingleton<IConnection>(sp =>
            {
                var factory = new ConnectionFactory
                {
                    Uri = new Uri(builder.Configuration.GetConnectionString("rabbitmq")),
                    AutomaticRecoveryEnabled = true,
                    NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
                };
                return factory.CreateConnectionAsync().GetAwaiter().GetResult();
            });

            builder.Services.AddScoped<IChannel>(sp =>
            {
                var connection = sp.GetRequiredService<IConnection>();
                return connection.CreateChannelAsync().GetAwaiter().GetResult();
            });

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
