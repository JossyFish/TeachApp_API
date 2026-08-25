using Auth.Domain.Enums;

namespace Auth.Application.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateToken(Guid userId, Role[] roles);
        string GenerateToken(Guid userId, Role role);
    }
}