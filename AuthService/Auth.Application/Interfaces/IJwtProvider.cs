using Auth.Domain.Enums;

namespace Auth.Application.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateAccessToken(Guid userId);
        string GenerateRefreshToken(Guid userId, Guid sessionId);
        Guid? ValidateRefreshToken(string token, out Guid? sessionId);
    }
}