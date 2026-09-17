namespace Auth.Domain.Models.Exceptions
{
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException()
            : base("Неверный email или пароль.")
        {
        }
    }
}
