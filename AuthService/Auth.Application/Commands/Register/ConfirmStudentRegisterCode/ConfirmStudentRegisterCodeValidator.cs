using FluentValidation;

namespace Auth.Application.Commands.Register.ConfirmStudentRegisterCode
{
    public class ConfirmStudentRegisterCodeValidator : AbstractValidator<ConfirmStudentRegisterCodeCommand>
    {
        public ConfirmStudentRegisterCodeValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Wrong email format")
                .MaximumLength(255).WithMessage("Email must not be higher than 255 characters");

            RuleFor(x => x.ConfirmationCode)
                .NotEmpty().WithMessage("ConfirmationCode is required")
                .Length(6).WithMessage("ConfirmationCode must be 6 characters")
                .Must(x => x.All(char.IsDigit)).WithMessage("ConfirmationCode must contain only digits");
        }
    }
}
