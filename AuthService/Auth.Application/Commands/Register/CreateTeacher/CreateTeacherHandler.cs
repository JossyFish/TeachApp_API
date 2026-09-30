using Auth.Application.Interfaces;
using Auth.Domain.Interfaces;
using Auth.Domain.Models.Cache;
using Auth.Domain.Models.Exceptions;
using MassTransit;
using MediatR;

namespace Auth.Application.Commands.Register.CreateTeacher
{
    public class CreateTeacherHandler : IRequestHandler<CreateTeacherCommand, Unit>
    {

        private readonly ICacheUsersRepository _cache;
        private readonly IUsersRepository _usersRepository;
        private readonly INumberProcessor _numberProcessor;
        private readonly IEmailMessageService _emailMessageService;
        private readonly IPublishEndpoint _publishEndpoint;

        public CreateTeacherHandler(IUsersRepository usersRepository, ICacheUsersRepository cache, IEmailMessageService emailMessageService, INumberProcessor numberProcessor, IPublishEndpoint publishEndpoint)
        {
            _cache = cache;
            _usersRepository = usersRepository;
            _emailMessageService = emailMessageService;
            _numberProcessor = numberProcessor;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Unit> Handle(CreateTeacherCommand command, CancellationToken cancellationToken)
        {
            var existingUser = await _usersRepository.GetByEmailAsync(command.Email, cancellationToken);
            if (existingUser != null)
                throw new UserAlreadyExistException(command.Email);

            await _cache.RemoveUserDataByEmailAsync<CreationTeacherData>(command.Email, cancellationToken);

            var hashPassword = _numberProcessor.Generate(command.Password);
            var confirmUserCode = _numberProcessor.GenerateConfirmCode();

            var creationData = new CreationTeacherData(
                id: Guid.NewGuid(),
                name: command.Name,
                lastName: command.LastName,
                email: command.Email,
                passwordHash: hashPassword,
                confirmationCode: confirmUserCode,
                expertiseIds: command.ExpertiseIds,
                experience: command.Experience,
                bio: command.Bio,
                subscriptionPlanId: command.SubscriptionPlanId,
                cardNumber: command.CardNumber,
                cardExpiry: command.CardExpiry,
                cardCvc: command.CardCvc
            );

            await _cache.SaveUserDataAsync<CreationTeacherData>(creationData.Id, creationData.Email, creationData, cancellationToken);

            var emailMessage = _emailMessageService.CreateConfirmationEmail(
                email: command.Email,
                firstName: command.Name,
                confirmationCode: confirmUserCode,
                userId: creationData.Id);

            await _publishEndpoint.Publish(emailMessage, ct);

            return Unit.Value;

        }

    }
}
