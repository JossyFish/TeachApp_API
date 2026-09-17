namespace Auth.Domain.Interfaces
{
    public interface ISessionCacheRepository
    {
        Task SaveAsync(Guid sessionId, Guid userId, TimeSpan ttl, CancellationToken ct);
        Task<Guid?> GetUserIdAsync(Guid sessionId, CancellationToken ct);
        Task RemoveAsync(Guid sessionId, CancellationToken ct);
        Task RemoveAllForUserAsync(Guid userId, CancellationToken ct);
    }
}
