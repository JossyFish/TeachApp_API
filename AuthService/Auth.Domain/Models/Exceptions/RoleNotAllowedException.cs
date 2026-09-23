using Auth.Domain.Enums;

namespace Auth.Domain.Models.Exceptions
{
    public class RoleNotAllowedException : Exception
    {
        public RoleNotAllowedException(string role)
            : base($"Роль '{role}' нельзя выбрать при регистрации через Google.")
        {
            Role = role;
        }

        public string Role { get; }
    }
}
