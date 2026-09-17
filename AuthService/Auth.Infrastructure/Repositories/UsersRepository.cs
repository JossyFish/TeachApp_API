using Auth.Domain.Entites;
using Auth.Domain.Enums;
using Auth.Domain.Interfaces;
using Auth.Domain.Models;
using Auth.Infrastructure.Data;
using AutoMapper;
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
            var userEntity = await _context.Users
                .AsNoTracking()
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

            if (userEntity == null)
            { return null; }

            return _mapper.Map<User>(userEntity);
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
