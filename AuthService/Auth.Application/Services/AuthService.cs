using Auth.Application.Dtos;
using Auth.Application.Interfaces;
using Auth.Domain.Models.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Auth.Application.Services
{
    internal class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly GitHubAuthOptions _options;
        private readonly ILogger<AuthService> _logger;

        public AuthService(HttpClient httpClient, IOptions<GitHubAuthOptions> options, ILogger<AuthService> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<GitHubUserInfo?> GetUserInfoAsync(string code, CancellationToken cancellationToken)
        {
            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://github.com/login/oauth/access_token")
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["client_id"] = _options.ClientId,
                    ["client_secret"] = _options.ClientSecret,
                    ["code"] = code
                })
            };
            tokenRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var tokenResponse = await _httpClient.SendAsync(tokenRequest, cancellationToken);
            if (!tokenResponse.IsSuccessStatusCode)
            {
                _logger.LogWarning("GitHub token exchange failed: {Status}", tokenResponse.StatusCode);
                return null;
            }

            var tokenData = await tokenResponse.Content.ReadFromJsonAsync<GitHubTokenResponse>(cancellationToken);
            if (tokenData?.AccessToken is null)
            {
                _logger.LogWarning("GitHub token exchange returned no access_token");
                return null;
            }

            var userRequest = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user");
            userRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenData.AccessToken);
            userRequest.Headers.UserAgent.ParseAdd("TeachApp");

            var userResponse = await _httpClient.SendAsync(userRequest, cancellationToken);
            if (!userResponse.IsSuccessStatusCode)
            {
                _logger.LogWarning("GitHub user fetch failed: {Status}", userResponse.StatusCode);
                return null;
            }

            var profile = await userResponse.Content.ReadFromJsonAsync<GitHubProfileResponse>(cancellationToken);
            if (profile is null) return null;

            var emailsRequest = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user/emails");
            emailsRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenData.AccessToken);
            emailsRequest.Headers.UserAgent.ParseAdd("TeachApp");

            var emailsResponse = await _httpClient.SendAsync(emailsRequest, cancellationToken);
            if (!emailsResponse.IsSuccessStatusCode)
            {
                _logger.LogWarning("GitHub emails fetch failed: {Status}", emailsResponse.StatusCode);
                return null;
            }

            var emails = await emailsResponse.Content.ReadFromJsonAsync<List<GitHubEmailResponse>>(cancellationToken);
            if (emails is null || emails.Count == 0) return null;

            var primaryEmail = emails.FirstOrDefault(e => e.Primary && e.Verified)
                ?? emails.FirstOrDefault(e => e.Verified);

            if (primaryEmail?.Email is null)
            {
                _logger.LogWarning("GitHub user has no verified email");
                return null;
            }

            return new GitHubUserInfo(
                Email: primaryEmail.Email,
                Name: profile.Name ?? profile.Login ?? "User",
                Login: profile.Login);
        }

}
}
