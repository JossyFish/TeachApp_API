using MediatR;

namespace Auth.Application.Commands.Register.ConfirmStudentRegisterCode
{
    public record ConfirmStudentRegisterCodeCommand : IRequest<string>
    {
        public string Email { get; set; } = string.Empty;
        public string ConfirmationCode { get; set; } = string.Empty;
    }
}
