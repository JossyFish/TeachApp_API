
namespace Auth.Domain.Models.Options
{
    public class RabbitMQOptions
    {
        public string ConnectionUrl { get; set; } = string.Empty;

        public string EmailQueueName { get; set; } = "email_queue";
    }
}