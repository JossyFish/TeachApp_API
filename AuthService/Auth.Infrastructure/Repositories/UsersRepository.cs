using Auth.Domain.Interfaces;
using Auth.Domain.Models;
using Auth.Infrastructure.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private AuthDBContext _context;
        private readonly IMapper _mapper;
        public UsersRepository(AuthDBContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
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


    }
}
