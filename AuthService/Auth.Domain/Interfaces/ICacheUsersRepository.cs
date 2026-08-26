
using Auth.Domain.Models.Cache;

namespace Auth.Domain.Interfaces
{
    public interface ICacheUsersRepository
    {
        Task SaveUserDataAsync<T>(Guid userId, string email, T data, CancellationToken cancellationToken)
            where T : class;  
        Task<T?> GetUserDataByEmailAsync<T>(string email, CancellationToken cancellationToken)
            where T : class;  
        Task<T?> GetUserDataByIdAsync<T>(Guid userId, CancellationToken cancellationToken)
            where T : class;  
        Task RemoveUserDataAsync<T>(Guid userId, CancellationToken cancellationToken)
            where T : class;  
        Task RemoveUserDataByEmailAsync<T>(string email, CancellationToken cancellationToken)
            where T : class;
    }
}