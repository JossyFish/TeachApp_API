using MediatR;

namespace Auth.Application.Commands.Delete.DeleteUser
{
    public class DeleteUserCommand : IRequest<Unit>
    {
        public string Email { get; set; } 
    }
}
