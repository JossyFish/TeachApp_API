using Communication.Domain.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using TeachApp.Contracts.Events;

namespace Communication.Infrastructure.Consumers
{
    public class EmailConsumer : IConsumer<EmailMessageEvent>
    {
        private readonly ILogger<EmailConsumer> _logger;
        private readonly IEmailSender _emailSender; 

        public EmailConsumer(ILogger<EmailConsumer> logger, IEmailSender emailSender)
        {
            _logger = logger;
            _emailSender = emailSender;
        }

        public async Task Consume(ConsumeContext<EmailMessageEvent> context)
        {
            var message = context.Message;

            _logger.LogInformation("Processing email for {Email}", message.To);

            await _emailSender.SendEmailAsync(
                message.To,
                message.Subject,
                message.Body,
                message.IsHtml
            );

            _logger.LogInformation("Email sent to {Email}", message.To);
        }
    }
}
