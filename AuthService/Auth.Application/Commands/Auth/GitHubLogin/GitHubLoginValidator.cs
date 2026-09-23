using Auth.Application.Commands.Auth.GoogleLogin;
using FluentValidation;

namespace Auth.Application.Commands.Auth.GitHubLogin
{
    public sealed class GitHubLoginValidator : AbstractValidator<GitHubLoginCommand>
    {
        public GitHubLoginValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("GitHub код обязателен.")
                .MaximumLength(4096).WithMessage("GitHub токен слишком длинный.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Роль обязательна.");

        }
    }
}
