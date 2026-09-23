namespace Auth.Domain.Models.Exceptions
{
    public class EmailNotVerifiedException : Exception
    {
        public EmailNotVerifiedException(string email)
            : base($"Email '{email}' не подтверждён в Google.")
        {
            Email = email;
        }

        public string Email { get; }
    }
}
