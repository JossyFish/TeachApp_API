using Auth.Domain.Models.Models;

namespace Auth.Domain.Interfaces
{
    public interface IUsersRepository
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task DeleteAsync(string email, CancellationToken cancellationToken);
        Task UpdateLastLoginAsync(Guid userId, CancellationToken cancellationToken);
        Task AddStudentAsync(User user, Student student, CancellationToken cancellationToken);

        Task<IReadOnlyList<Domain.Models.Models.Expertise?>> GetExpertisesAsync(CancellationToken cancellationToken);
        Task<IReadOnlyList<SubscriptionPlan?>> GetSubscriptionPlansAsync(CancellationToken cancellationToken);
    }
}