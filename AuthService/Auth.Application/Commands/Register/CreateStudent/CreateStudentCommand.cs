using MediatR;

namespace Auth.Application.Commands.Register.CreateStudent
{
    public class CreateStudentCommand : IRequest<Unit>
    {
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

    }
}
