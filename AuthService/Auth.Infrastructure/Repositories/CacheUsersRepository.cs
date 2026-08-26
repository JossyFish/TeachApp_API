using Auth.Domain.Interfaces;
using Auth.Domain.Models.Cache;
using Auth.Domain.Models.Options;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Auth.Infrastructure.Repositories
{
    public class CacheUsersRepository : ICacheUsersRepository
    {
        private readonly HybridCache _cache;
        private readonly ILogger<CacheUsersRepository> _logger;
        private readonly CacheOptions _options;

        public CacheUsersRepository(HybridCache cache, ILogger<CacheUsersRepository> logger, IOptions<CacheOptions> options)
        {
            _cache = cache;
            _logger = logger;
            _options = options.Value;
        }

        private HybridCacheEntryOptions UserCacheOptions => new()
        {
            LocalCacheExpiration = _options.UserLocalCacheExpiration,
            Expiration = _options.UserExpiration
        };

        private HybridCacheEntryOptions ResetPasswordCacheOptions => new()
        {
            LocalCacheExpiration = _options.ResetPasswordLocalCacheExpiration,
            Expiration = _options.ResetPasswordExpiration
        };

        private async Task SaveAsync<T>(string key, T data, HybridCacheEntryOptions options, CancellationToken cancellationToken)
        {
            try
            {
                await _cache.SetAsync(key, data, options);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to cache data with key: {Key}", key);
            }
        }

        private async Task<T?> GetAsync<T>(string key, HybridCacheEntryOptions options, CancellationToken cancellationToken)
        {
            try
            {
                return await _cache.GetOrCreateAsync<T>(
                    key,
                    async cancel => await Task.FromResult<T?>(default),
                    options);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to get data with key: {Key}", key);
                return default;
            }
        }


        public async Task SaveUserDataAsync<T>(Guid userId, string email, T data, CancellationToken cancellationToken)
            where T : class
        {
            var key = GetUserKey(userId);
            var emailKey = GetEmailKey(email);

            await SaveAsync(key, data, UserCacheOptions, cancellationToken);
            await SaveAsync(emailKey, userId.ToString(), UserCacheOptions, cancellationToken);
        }

        public async Task<T?> GetUserDataByEmailAsync<T>(string email, CancellationToken cancellationToken)
            where T : class
        {
            var emailKey = GetEmailKey(email);
            var userIdStr = await GetAsync<string>(emailKey, UserCacheOptions, cancellationToken);

            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
                return null;

            var userKey = GetUserKey(userId);
            return await GetAsync<T>(userKey, UserCacheOptions, cancellationToken);
        }

        public async Task<T?> GetUserDataByIdAsync<T>(Guid userId, CancellationToken cancellationToken)
            where T : class
        {
            var key = GetUserKey(userId);
            return await GetAsync<T>(key, UserCacheOptions, cancellationToken);
        }

        public async Task RemoveUserDataAsync<T>(Guid userId, CancellationToken cancellationToken)
            where T : class
        {
            var user = await GetUserDataByIdAsync<T>(userId, cancellationToken);
            if (user != null)
            {
                // Получаем email через рефлексию (если у T есть свойство Email)
                var emailProperty = typeof(T).GetProperty("Email");
                if (emailProperty != null)
                {
                    var email = emailProperty.GetValue(user) as string;
                    if (!string.IsNullOrEmpty(email))
                    {
                        await _cache.RemoveAsync(GetEmailKey(email), cancellationToken);
                    }
                }

                await _cache.RemoveAsync(GetUserKey(userId), cancellationToken);
            }
        }

        public async Task RemoveUserDataByEmailAsync<T>(string email, CancellationToken cancellationToken)
            where T : class
        {
            try
            {
                var emailKey = GetEmailKey(email);
                var userIdStr = await GetAsync<string>(emailKey, UserCacheOptions, cancellationToken);

                await _cache.RemoveAsync(emailKey, cancellationToken);

                if (!string.IsNullOrEmpty(userIdStr) && Guid.TryParse(userIdStr, out var userId))
                {
                    await _cache.RemoveAsync(GetUserKey(userId), cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to remove user data by email: {Email}", email);
            }
        }


        private static string GetUserKey(Guid userId) => $"user_id:{userId}";
        private static string GetEmailKey(string email) => $"user_email:{email}";
        private static string GetResetPasswordKey(Guid userId) => $"reset_password:{userId}";
        private static string GetChangeEmailKey(Guid userId) => $"change_email:{userId}";


    }
}
