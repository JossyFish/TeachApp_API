using Auth.Domain.Enums;
using MediatR;

namespace Auth.Application.Commands.Register.CreateTeacher
{
    public class CreateTeacherCommand : IRequest<Unit>
    {
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public List<Expertise> Expertises { get; set; } = [];  
        public string Experience { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;



    }
}
