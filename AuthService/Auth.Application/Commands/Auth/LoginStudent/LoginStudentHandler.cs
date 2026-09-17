using Auth.Application.Dtos;
using Auth.Application.Interfaces;
using Auth.Application.Services;
using Auth.Domain.Enums;
using Auth.Domain.Interfaces;
using Auth.Domain.Models;
using Auth.Domain.Models.Exceptions;
using Auth.Domain.Models.Options;
using MediatR;
using Microsoft.Extensions.Options;
using System.Security.Authentication;

namespace Auth.Application.Commands.Auth.LoginStudent
{
    public sealed class LoginStudentHandler : IRequestHandler<LoginStudentCommand, LoginResponse>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly INumberProcessor _numberProcessor;
        private readonly IJwtProvider _jwtProvider;
        private readonly ISessionCacheRepository _sessionCacheRepository;
        private readonly JwtOptions _jwtOptions;

        public LoginStudentHandler(IUsersRepository usersRepository, INumberProcessor numberProcessor, IJwtProvider jwtProvider, IPermissionService permissionService, ISessionCacheRepository sessionCacheRepository,
            IOptions<JwtOptions> jwtOptions)
        {
            _usersRepository = usersRepository;
            _numberProcessor = numberProcessor;
            _jwtProvider = jwtProvider;
            _sessionCacheRepository = sessionCacheRepository;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<LoginResponse> Handle(LoginStudentCommand command, CancellationToken cancellationToken)
        {
            var user = await _usersRepository.GetByEmailAsync(command.Email, cancellationToken);

            if (user == null)
                throw new UserNotFoundException(command.Email);

            if (!_numberProcessor.Verify(command.Password, user.PasswordHash))
                throw new InvalidCredentialsException();

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
                Roles: user.Roles
            );

        }


    }
}
