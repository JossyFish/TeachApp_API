
using Auth.API.Exceptions;
using Auth.Application;
using Auth.Application.Extensions;
using Auth.Domain.Models.Options;
using Auth.Infrastructure;
using Auth.Infrastructure.Data;
using Microsoft.Extensions.Options;

namespace Auth.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Logging.AddDebug();



        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        builder.Services.AddProblemDetails();

        builder.Services.AddHybridCache();

        builder.Services.AddAuth();

        builder.Services.Configure<AuthorizationOptions>(
              builder.Configuration.GetSection(nameof(AuthorizationOptions)));
        builder.Services.AddSingleton(provider =>
            provider.GetRequiredService<IOptions<AuthorizationOptions>>().Value);
        builder.Services.Configure<CacheOptions>(builder.Configuration.GetSection(nameof(CacheOptions)));
        builder.Services.Configure<RabbitMQOptions>(builder.Configuration.GetSection(nameof(RabbitMQOptions)));

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseExceptionHandler();
        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
