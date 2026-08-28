using Auth.Application.Interfaces;
using Auth.Domain.Models.Events;
using Microsoft.Extensions.Logging;

namespace Auth.Application.Services
{
    public class EmailMessageService : IEmailMessageService
    {
        private readonly ILogger<EmailMessageService> _logger;

        public EmailMessageService(ILogger<EmailMessageService> logger)
        {
            _logger = logger;
        }


        public EmailMessageEvent CreateConfirmationEmail(string email, string firstName, string confirmationCode, Guid userId)
        {
         
            var body = EmailTemplateHelper.GetConfirmationEmail(firstName, confirmationCode);

            _logger.LogInformation("Created confirmation email for {Email}", email);

            return new EmailMessageEvent
            {
                To = email,
                Subject = "Подтверждение регистрации в Teach",
                Body = body,
                IsHtml = true
            };
        }

        public EmailMessageEvent CreateWelcomeEmail(string email, string firstName)
        {
            var body = EmailTemplateHelper.GetWelcomeEmail(firstName);

            _logger.LogInformation("Created welcome email for {Email}", email);

            return new EmailMessageEvent
            {
                To = email,
                Subject = "Добро пожаловать в Teach! 🎓",
                Body = body,
                IsHtml = true
            };
        }


    }
}
