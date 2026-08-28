namespace Auth.Domain.Models.Exceptions
{
    public class UserAlreadyExistException : Exception
    {
        public string Email { get; }

        public UserAlreadyExistException(string email)
            : base($"User with email '{email}' already exists")
        {
            Email = email;
        }
    }
}
