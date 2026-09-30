using Auth.Application.Dtos;
using MediatR;

namespace Auth.Application.Commands.Register.ConfirmTeacherRegisterCode
{
    public class ConfirmTeacherRegisterCodeCommand : IRequest<LoginResponse>
    {
        public string Email { get; set; } = string.Empty;
        public string ConfirmationCode { get; set; } = string.Empty;
    }
}
