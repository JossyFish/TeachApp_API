namespace Auth.Domain.Models.Exceptions
{
    public class InvalidGitHubTokenException : Exception
    {
        public InvalidGitHubTokenException()
            : base("Невалидный GitHub код.")
        {
        }
    }
}
