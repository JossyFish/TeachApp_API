using TeachApp.Contracts.Events;

namespace Auth.Application.Interfaces
{
    public interface IEmailMessageService
    {
        EmailMessageEvent CreateConfirmationEmail(string email, string firstName, string confirmationCode, Guid userId);

        EmailMessageEvent CreateWelcomeEmail(string email, string firstName);
    }
}
