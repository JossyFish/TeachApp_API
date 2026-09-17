using Auth.Domain.Interfaces;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace Auth.Infrastructure.Repositories
{
    public class SessionCacheRepository : ISessionCacheRepository
    {
        private readonly HybridCache _cache;
        private readonly ILogger<SessionCacheRepository> _logger;

        public SessionCacheRepository(HybridCache cache, ILogger<SessionCacheRepository> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task SaveAsync(Guid sessionId, Guid userId, TimeSpan ttl, CancellationToken ct)
        {
            try
            {
                var options = new HybridCacheEntryOptions
                {
                    Expiration = ttl,                       
                    LocalCacheExpiration = TimeSpan.FromMinutes(5) 
                };

                await _cache.SetAsync(GetSessionKey(sessionId), userId, options, cancellationToken: ct);
                await _cache.SetAsync(GetUserSessionsKey(userId), sessionId, options, cancellationToken: ct);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to save session {SessionId}", sessionId);
                throw;  
            }
        }
        public async Task<Guid?> GetUserIdAsync(Guid sessionId, CancellationToken ct)
        {
            try
            {
                var key = GetSessionKey(sessionId);
                var userId = await _cache.GetOrCreateAsync<Guid?>(
                    key,
                    _ => ValueTask.FromResult<Guid?>(null),
                    cancellationToken: ct);

                return userId;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to get session {SessionId}", sessionId);
                return null;
            }
        }
        public async Task RemoveAsync(Guid sessionId, CancellationToken ct)
        {
            try
            {
                var userId = await GetUserIdAsync(sessionId, ct);
                await _cache.RemoveAsync(GetSessionKey(sessionId), ct);

                if (userId.HasValue)
                    await _cache.RemoveAsync(GetUserSessionsKey(userId.Value), ct);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to remove session {SessionId}", sessionId);
            }
        }

        public async Task RemoveAllForUserAsync(Guid userId, CancellationToken ct)
        {
            try
            {
                await _cache.RemoveAsync(GetUserSessionsKey(userId), ct);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to remove all sessions for user {UserId}", userId);
            }
        }

        private static string GetSessionKey(Guid sessionId) => $"session:{sessionId}";
        private static string GetUserSessionsKey(Guid userId) => $"user_sessions:{userId}";

    }
}
