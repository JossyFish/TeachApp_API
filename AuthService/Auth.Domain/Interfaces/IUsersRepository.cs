using Auth.Domain.Models;

namespace Auth.Domain.Interfaces
{
    public interface IUsersRepository
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task AddStudentAsync(User user, Student student, CancellationToken cancellationToken);
    }
}