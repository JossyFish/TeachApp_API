using Auth.Domain.Interfaces;
using Auth.Domain.Models.Exceptions;
using MediatR;

namespace Auth.Application.Commands.Delete.DeleteUser
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IUsersRepository _usersRepository;

        public DeleteUserHandler(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<Unit> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _usersRepository.GetByEmailAsync(command.Email, cancellationToken)
                ?? throw new UserNotFoundException(command.Email);

            await _usersRepository.DeleteAsync(command.Email, cancellationToken);

            return Unit.Value;
        }

    }
}
