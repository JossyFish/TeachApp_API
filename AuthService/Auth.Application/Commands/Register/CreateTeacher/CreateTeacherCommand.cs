using MediatR;

namespace Auth.Application.Commands.Register.CreateTeacher
{
    public class CreateTeacherCommand : IRequest<Unit>
    {
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public List<int> ExpertiseIds { get; set; } = [];
        public string Experience { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;

        public string CardNumber { get; set; } = string.Empty;
        public string CardExpiry { get; set; } = string.Empty;
        public string CardCvc { get; set; } = string.Empty;

        public int SubscriptionPlanId { get; set; }


    }
}
