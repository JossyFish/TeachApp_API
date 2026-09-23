using Auth.Application.Dtos;
using Auth.Application.Interfaces;
using Auth.Domain.Enums;
using Auth.Domain.Interfaces;
using Auth.Domain.Models;
using Auth.Domain.Models.Exceptions;
using Auth.Domain.Models.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace Auth.Application.Commands.Auth.GitHubLogin
{
    public sealed class GitHubLoginHandler : IRequestHandler<GitHubLoginCommand, LoginResponse>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly ISessionCacheRepository _sessionCacheRepository;
        private readonly IAuthService _authService;
        private readonly JwtOptions _jwtOptions;

        public GitHubLoginHandler(IUsersRepository usersRepository, IJwtProvider jwtProvider, ISessionCacheRepository sessionCacheRepository, IAuthService authService, IOptions<JwtOptions> jwtOptions)
        {
            _usersRepository = usersRepository;
            _jwtProvider = jwtProvider;
            _sessionCacheRepository = sessionCacheRepository;
            _authService = authService;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<LoginResponse> Handle(GitHubLoginCommand command, CancellationToken cancellationToken)
        {
            var info = await _authService.GetUserInfoAsync(command.Code, cancellationToken)
                    ?? throw new InvalidGitHubTokenException();

            var user = await _usersRepository.GetByEmailAsync(info.Email, cancellationToken);

            if (user == null)
            {
                if (command.Role != Role.Student)
                    throw new RoleNotAllowedException(command.Role.ToString());

                user = new User(
                    id: Guid.NewGuid(),
                    name: info.Name,
                    lastName: string.Empty,
                    email: info.Email,
                    passwordHash: string.Empty,
                    roles: new List<Role> { Role.Student },
                    cardLastDigits: null,
                    cardBrand: null,
                    isActive: true,
                    createdAt: DateTime.UtcNow,
                    updatedAt: DateTime.UtcNow,
                    lastLogin: DateTime.UtcNow);

                var student = new Student(
                    userId: user.Id,
                    enrolledCoursesCount: 0,
                    completedCoursesCount: 0,
                    learningHours: 0,
                    certificatesCount: 0,
                    streakDays: 0);

                await _usersRepository.AddStudentAsync(user, student, cancellationToken);
            }
            else
            {
                if (!user.Roles.Contains(command.Role))
                    throw new RoleMismatchException(command.Role.ToString());

                await _usersRepository.UpdateLastLoginAsync(user.Id, cancellationToken);
            }

            var sessionId = Guid.NewGuid();
            var accessToken = _jwtProvider.GenerateAccessToken(user.Id);
            var refreshToken = _jwtProvider.GenerateRefreshToken(user.Id, sessionId);

            await _sessionCacheRepository.SaveAsync(sessionId, user.Id, TimeSpan.FromDays(_jwtOptions.RefreshExpiresDays), cancellationToken);

            return new LoginResponse(
                AccessToken: accessToken,
                RefreshToken: refreshToken,
                UserId: user.Id,
                Email: user.Email,
                Name: user.Name,
                LastName: user.LastName,
                Roles: user.Roles);

        }

    }
}
