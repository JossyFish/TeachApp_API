namespace Auth.Domain.Models.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string email)
            : base($"Пользователь с email '{email}' не найден.")
        {
            Email = email;
        }

        public UserNotFoundException(Guid userId)
            : base($"Пользователь с Id '{userId}' не найден.")
        {
            UserId = userId;
        }

        public string? Email { get; }
        public Guid? UserId { get; }
    }
}
