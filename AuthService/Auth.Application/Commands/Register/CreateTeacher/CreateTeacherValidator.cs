using FluentValidation;

namespace Auth.Application.Commands.Register.CreateTeacher
{
    public class CreateTeacherValidator : AbstractValidator<CreateTeacherCommand>
    {
        public CreateTeacherValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(100).WithMessage("Last name must not exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.")
                .MaximumLength(255).WithMessage("Email must not exceed 255 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(100).WithMessage("Password must not exceed 100 characters.");

            RuleFor(x => x.ExpertiseIds)
                .NotEmpty().WithMessage("Select at least one area of expertise.");

            RuleForEach(x => x.ExpertiseIds)
                .GreaterThan(0).WithMessage("Expertise ID must be greater than 0.");

            RuleFor(x => x.Experience)
                .NotEmpty().WithMessage("Experience is required.")
                .MaximumLength(50).WithMessage("Experience must not exceed 50 characters.");

            RuleFor(x => x.Bio)
                .NotEmpty().WithMessage("Bio is required.")
                .MaximumLength(1000).WithMessage("Bio must not exceed 1000 characters.");

            RuleFor(x => x.SubscriptionPlanId)
                .GreaterThan(0).WithMessage("Subscription plan is required.");

            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("Card number is required.")
                .Must(BeValidCardNumber).WithMessage("Invalid card number.");

            RuleFor(x => x.CardExpiry)
                .NotEmpty().WithMessage("Card expiry date is required.")
                .Matches(@"^(0[1-9]|1[0-2])\/\d{2}$")
                .WithMessage("Expiry date must be in MM/YY format.");

            RuleFor(x => x.CardCvc)
                .NotEmpty().WithMessage("CVC is required.")
                .Length(3, 4).WithMessage("CVC must be 3 or 4 digits.")
                .Matches(@"^\d{3,4}$").WithMessage("CVC must contain only digits.");
        }

        private static bool BeValidCardNumber(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber)) return false;
            var digits = cardNumber.Replace(" ", "").Replace("-", "");
            return digits.Length is >= 13 and <= 19 && digits.All(char.IsDigit);
        }
    }
}
