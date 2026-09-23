using Auth.Application.Dtos;
using Auth.Application.Interfaces;
using Auth.Domain.Enums;
using Auth.Domain.Interfaces;
using Auth.Domain.Models;
using Auth.Domain.Models.Exceptions;
using Auth.Domain.Models.Options;
using Google.Apis.Auth;
using MediatR;
using Microsoft.Extensions.Options;

namespace Auth.Application.Commands.Auth.GoogleLogin
{
    public sealed class GoogleLoginHandler : IRequestHandler<GoogleLoginCommand, LoginResponse>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly ISessionCacheRepository _sessionCacheRepository;
        private readonly GoogleAuthOptions _googleOptions;
        private readonly JwtOptions _jwtOptions;

        public GoogleLoginHandler(IUsersRepository usersRepository, IJwtProvider jwtProvider, ISessionCacheRepository sessionCacheRepository, IOptions<GoogleAuthOptions> googleOptions,
                     IOptions<JwtOptions> jwtOptions)
        {
            _usersRepository = usersRepository;
            _jwtProvider = jwtProvider;
            _sessionCacheRepository = sessionCacheRepository;
            _googleOptions = googleOptions.Value;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<LoginResponse> Handle(GoogleLoginCommand command, CancellationToken cancellationToken)
        {
            GoogleJsonWebSignature.Payload payload;
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _googleOptions.ClientId }
                };
                payload = await GoogleJsonWebSignature.ValidateAsync(command.IdToken, settings);
            }
            catch (InvalidJwtException)
            {
                throw new InvalidGoogleTokenException();
            }

            if (!payload.EmailVerified)
                throw new EmailNotVerifiedException(payload.Email);

            var user = await _usersRepository.GetByEmailAsync(payload.Email, cancellationToken);

            if (user == null)
            {
                if (command.Role != Role.Student)
                    throw new RoleNotAllowedException(command.Role.ToString());

                user = new User(
                        id: Guid.NewGuid(),
                        name: payload.GivenName ?? payload.Name ?? "User",
                        lastName: payload.FamilyName ?? string.Empty,
                        email: payload.Email,
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

            await _sessionCacheRepository.SaveAsync(
                sessionId: sessionId,
                userId: user.Id,
                ttl: TimeSpan.FromDays(_jwtOptions.RefreshExpiresDays),
                ct: cancellationToken);

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
