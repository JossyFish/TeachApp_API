using Auth.Application.Dtos;
using MediatR;

namespace Auth.Application.Commands.Auth.LoginStudent
{
    public record class LoginStudentCommand : IRequest<LoginResponse>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

}
