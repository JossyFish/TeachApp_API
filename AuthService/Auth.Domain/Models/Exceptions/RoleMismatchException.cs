namespace Auth.Domain.Models.Exceptions
{
    public class RoleMismatchException : Exception
    {
        public RoleMismatchException(string role)
            : base($"У вашего аккаунта нет роли '{role}'.")
        {
            Role = role;
        }

        public string Role { get; }
    }
}
