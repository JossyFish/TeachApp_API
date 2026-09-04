using Auth.Application.Commands.Register.CreateStudent;
using Auth.Application.Extensions;
using Auth.Application.Interfaces;
using Auth.Application.Services;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace Auth.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(typeof(CreateStudentCommand).Assembly);


            services.AddValidatorsFromAssemblyContaining<CreateStudentValidator>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


            services.AddScoped<IEmailMessageService, EmailMessageService>();
            services.AddScoped<INumberProcessor, NumberProcessor>();

            return services;
        }
    }
}
