using FluentValidation;

namespace Auth.Application.Commands.Auth.GoogleLogin
{
    public sealed class GoogleLoginValidator : AbstractValidator<GoogleLoginCommand>
    {
        public GoogleLoginValidator() 
        {
            RuleFor(x => x.IdToken)
                .NotEmpty().WithMessage("Google токен обязателен.")
                .MaximumLength(4096).WithMessage("Google токен слишком длинный.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Роль обязательна.");

        }
    }
}
