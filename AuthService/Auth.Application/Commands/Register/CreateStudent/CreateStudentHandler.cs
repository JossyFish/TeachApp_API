using Auth.Application.Interfaces;
using Auth.Domain.Exceptions;
using Auth.Domain.Interfaces;
using Auth.Domain.Models.Cache;
using MediatR;

namespace Auth.Application.Commands.Register.CreateStudent
{
    public class CreateStudentHandler : IRequestHandler<CreateStudentCommand, Unit>
    {
        ICacheUsersRepository _cache;
        IUsersRepository _usersRepository;
        INumberProcessor _numberProcessor;

        public CreateStudentHandler(IUsersRepository usersRepository, ICacheUsersRepository cache, INumberProcessor numberProcessor)
        {
            _cache = cache;
            _usersRepository = usersRepository;
            _numberProcessor = numberProcessor;
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

            await _emailService.SendConfirmationEmailAsync(command.Email, confirmUserCode, confirmationLink, cancellationToken);

            return Unit.Value;
        }


    }
}
