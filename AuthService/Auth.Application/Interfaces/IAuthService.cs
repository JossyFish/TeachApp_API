using Auth.Application.Dtos;

namespace Auth.Application.Interfaces
{
    public interface IAuthService
    {
        Task<GitHubUserInfo?> GetUserInfoAsync(string code, CancellationToken cancellationToken);
    }
}