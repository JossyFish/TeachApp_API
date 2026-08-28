using Auth.Application.Interfaces;
using Auth.Application.Services;
using Auth.Domain.Enums;
using Auth.Domain.Interfaces;
using Auth.Domain.Models;
using Auth.Domain.Models.Cache;
using Auth.Domain.Models.Exceptions;
using MassTransit;
using MediatR;

namespace Auth.Application.Commands.Register.ConfirmStudentRegisterCode
{
    public sealed class ConfirmStudentRegisterCodeHandler : IRequestHandler<ConfirmStudentRegisterCodeCommand, string>
    {
        private readonly ICacheUsersRepository _cache;
        private readonly IUsersRepository _usersRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IEmailMessageService _emailMessageService;
        private readonly IPublishEndpoint _publishEndpoint;

        public ConfirmStudentRegisterCodeHandler(ICacheUsersRepository cache, IUsersRepository usersRepository, IJwtProvider jwtProvider, IEmailMessageService emailMessageService, IPublishEndpoint publishEndpoint)
        {
            _cache = cache;
            _usersRepository = usersRepository;
            _jwtProvider = jwtProvider;
            _emailMessageService = emailMessageService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<string> Handle(ConfirmStudentRegisterCodeCommand command, CancellationToken cancellationToken)
        {
            var existingUser = await _usersRepository.GetByEmailAsync(command.Email, cancellationToken);
            if (existingUser != null)
                throw new UserAlreadyExistException(command.Email);

            var userCache = await _cache.GetUserDataByEmailAsync<CreationStudentData>(command.Email, cancellationToken);

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
                roles: new List<string> { Role.Student.ToString() },
                cardLastDigits: null,
                cardBrand: null,
                isActive: true,
                createdAt: DateTime.UtcNow,
                updatedAt: DateTime.UtcNow,
                lastLogin: DateTime.UtcNow
                );

            var student = new Student(
                userId: userCache.Id,
                enrolledCoursesCount: 0,
                completedCoursesCount: 0,
                learningHours: 0,
                certificatesCount: 0,
                streakDays: 0
                );

            await _usersRepository.AddStudentAsync(user, student, cancellationToken);
            await _cache.RemoveUserDataAsync<CreationStudentData>(user.Id, cancellationToken);

            var token = _jwtProvider.GenerateToken(user.Id, Role.Student);

            var welcomeMessage = _emailMessageService.CreateWelcomeEmail(
                email: user.Email,
                firstName: user.Name
            );

            await _publishEndpoint.Publish(welcomeMessage, cancellationToken);

            return token;
        }

    }
}
