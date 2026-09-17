using Auth.Application.Interfaces;
using Auth.Domain.Interfaces;
using Auth.Domain.Models.Cache;
using Auth.Domain.Models.Exceptions;
using MediatR;
using MassTransit;

namespace Auth.Application.Commands.Register.CreateStudent
{
    public class CreateStudentHandler : IRequestHandler<CreateStudentCommand, Unit>
    {
        private readonly ICacheUsersRepository _cache;
        private readonly IUsersRepository _usersRepository;
        private readonly INumberProcessor _numberProcessor;
        private readonly IEmailMessageService _emailMessageService;
        private readonly IPublishEndpoint _publishEndpoint;

        public CreateStudentHandler(IUsersRepository usersRepository, ICacheUsersRepository cache, IEmailMessageService emailMessageService, INumberProcessor numberProcessor, IPublishEndpoint publishEndpoint)
        {
            _cache = cache;
            _usersRepository = usersRepository;
            _emailMessageService = emailMessageService;
            _numberProcessor = numberProcessor;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Unit> Handle(CreateStudentCommand command, CancellationToken cancellationToken)
        {
            var existingUser = await _usersRepository.GetByEmailAsync(command.Email, cancellationToken);
            if (existingUser != null)
                throw new UserAlreadyExistException(command.Email);

            await _cache.RemoveUserDataByEmailAsync<CreationStudentData>(command.Email, cancellationToken);

            var hashPassword = _numberProcessor.Generate(command.Password);
            var confirmUserCode = _numberProcessor.GenerateConfirmCode();

            var creationStudentData = new CreationStudentData(
             id: Guid.NewGuid(),
             name: command.Name,
             lastName: command.LastName,
             email: command.Email,
             passwordHash: hashPassword,
             confirmationCode: confirmUserCode
            );

            await _cache.SaveUserDataAsync<CreationStudentData>(creationStudentData.Id, creationStudentData.Email, creationStudentData, cancellationToken);

            var emailMessage = _emailMessageService.CreateConfirmationEmail(
              email: command.Email,
              firstName: command.Name,
              confirmationCode: confirmUserCode,
              userId: creationStudentData.Id
            );

            await _publishEndpoint.Publish(emailMessage, cancellationToken);

            return Unit.Value;
        }


    }
}
