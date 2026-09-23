namespace Auth.Domain.Models.Exceptions
{
    public class InvalidGoogleTokenException : Exception
    {
        public InvalidGoogleTokenException()
            : base("Невалидный Google токен.")
        {
        }

        public InvalidGoogleTokenException(string message)
            : base(message)
        {
        }
    }
}
