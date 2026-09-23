using Auth.Domain.Entites;
using Auth.Domain.Enums;
using Auth.Domain.Interfaces;
using Auth.Domain.Models;
using Auth.Infrastructure.Data;
using AutoMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Auth.Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private AuthDBContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersRepository> _logger;

        public UsersRepository(AuthDBContext context, IMapper mapper, ILogger<UsersRepository> logger) 
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;          
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Email == email)
                .Select(u => new User
                {
                    Id = u.Id,
                    Name = u.Name,
                    LastName = u.LastName,
                    Email = u.Email,
                    PasswordHash = u.PasswordHash,
                    CardLastDigits = u.CardLastDigits,
                    CardBrand = u.CardBrand,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = u.UpdatedAt,
                    LastLogin = u.LastLogin,
                    Roles = u.Roles.Select(r => (Role)r.Id).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task UpdateLastLoginAsync(Guid userId, CancellationToken cancellationToken)
        {
            await _context.Users
                            .Where(u => u.Id == userId)
                            .ExecuteUpdateAsync(s => s
                            .SetProperty(u => u.LastLogin, DateTime.UtcNow), cancellationToken);
        }

        public async Task AddStudentAsync(User user, Student student, CancellationToken cancellationToken)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var roleEntity = await _context.Roles
                    .SingleOrDefaultAsync(r => r.Id == (int)Role.Student, cancellationToken);

                if (roleEntity == null)
                    throw new InvalidOperationException("Student role not found");

                var userEntity = _mapper.Map<UserEntity>(user);
                userEntity.Roles.Add(roleEntity);

                await _context.Users.AddAsync(userEntity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken); 

                var studentEntity = _mapper.Map<StudentEntity>(student);
                studentEntity.UserId = userEntity.Id; 

                await _context.StudentProfiles.AddAsync(studentEntity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, ex.Message);
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }


    }
}
