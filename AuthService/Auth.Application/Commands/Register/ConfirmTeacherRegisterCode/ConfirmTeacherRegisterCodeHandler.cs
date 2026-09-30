using Auth.Application.Commands.Register.ConfirmStudentRegisterCode;
using Auth.Application.Dtos;
using Auth.Application.Interfaces;
using Auth.Domain.Enums;
using Auth.Domain.Interfaces;
using Auth.Domain.Models.Cache;
using Auth.Domain.Models.Exceptions;
using Auth.Domain.Models.Models;
using Auth.Domain.Models.Options;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Options;

namespace Auth.Application.Commands.Register.ConfirmTeacherRegisterCode
{
    public sealed class ConfirmTeacherRegisterCodeHandler : IRequestHandler<ConfirmTeacherRegisterCodeCommand, LoginResponse>
    {
        private readonly ICacheUsersRepository _cache;
        private readonly IUsersRepository _usersRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IEmailMessageService _emailMessageService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISessionCacheRepository _sessionCacheRepository;
        private readonly JwtOptions _jwtOptions;
        public ConfirmTeacherRegisterCodeHandler(ICacheUsersRepository cache, IUsersRepository usersRepository, IJwtProvider jwtProvider, IEmailMessageService emailMessageService, IPublishEndpoint publishEndpoint,
            ISessionCacheRepository sessionCacheRepository, IOptions<JwtOptions> jwtOptions)
        {
            _cache = cache;
            _usersRepository = usersRepository;
            _jwtProvider = jwtProvider;
            _emailMessageService = emailMessageService;
            _publishEndpoint = publishEndpoint;
            _sessionCacheRepository = sessionCacheRepository;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<LoginResponse> Handle(ConfirmTeacherRegisterCodeCommand command, CancellationToken cancellationToken)
        {
            var existingUser = await _usersRepository.GetByEmailAsync(command.Email, cancellationToken);
            if (existingUser != null)
                throw new UserAlreadyExistException(command.Email);

            var userCache = await _cache.GetUserDataByEmailAsync<CreationTeacherData>(command.Email, cancellationToken);

            if (userCache == null)
                throw new ConfirmCodeExpiredException(command.ConfirmationCode);

            if (userCache.ConfirmationCode != command.ConfirmationCode)
                throw new InvalidConfirmCodeException(command.Email);

            var user = new User(
                id: userCache.Id,
                name: userCache.Name,
                lastName: userCache.LastName,
                email: userCache.Email,
                passwordHash: userCache.PasswordHash,
                roles: new List<Role> { Role.Student },
                cardLastDigits: null,
                cardBrand: null,
                isActive: true,
                createdAt: DateTime.UtcNow,
                updatedAt: DateTime.UtcNow,
                lastLogin: DateTime.UtcNow
            );

            var subscriptionDuration = await _usersRepository.GetSubscriptionDuration(userCache.SubscriptionPlanId, cancellationToken);
            var cardLast4 = userCache.CardNumber.Length >= 4 ? userCache.CardNumber[^4..] : userCache.CardNumber;

            var teacher = new Teacher(
                 userId: userCache.Id,
                 expertiseIds: userCache.ExpertiseIds,
                 experience: userCache.Experience,
                 bio: userCache.Bio,
                 subscriptionPlanId: userCache.SubscriptionPlanId,
                 subscriptionExpiresAt: DateTime.UtcNow.AddDays(subscriptionDuration),
                 cardLastDigits: cardLast4,
                 isActive: true);

            await _usersRepository.AddTeacherAsync(user, teacher, cancellationToken);
            await _cache.RemoveUserDataAsync<CreationTeacherData>(user.Id, cancellationToken);

            var sessionId = Guid.NewGuid();

            var accessToken = _jwtProvider.GenerateAccessToken(user.Id);
            var refreshToken = _jwtProvider.GenerateRefreshToken(user.Id, sessionId);

            await _sessionCacheRepository.SaveAsync(
                                         sessionId: sessionId,
                                         userId: user.Id,
                                         ttl: TimeSpan.FromDays(_jwtOptions.RefreshExpiresDays),
                                         ct: cancellationToken);

            var welcomeMessage = _emailMessageService.CreateWelcomeEmail(
              email: user.Email,
              firstName: user.Name
            );

            await _publishEndpoint.Publish(welcomeMessage, cancellationToken);

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
