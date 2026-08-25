using MediatR;

namespace Auth.Application.Commands.Register.CreateStudent
{
    public class CreateStudentHandler : IRequestHandler<CreateStudentCommand, Unit>
    {
        public CreateStudentHandler()
        {

        }

        public async Task<Unit> Handle(CreateStudentCommand command, CancellationToken cancellationToken)
        {
            var existingUser = await _usersRepository.GetByEmailAsync(command.Email, cancellationToken);
            if (existingUser != null)
                throw new UserAlreadyExistException(command.Email);


            return Unit.Value;
        }


    }
}
