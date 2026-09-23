using Auth.Application.Dtos;
using Auth.Domain.Enums;
using MediatR;

namespace Auth.Application.Commands.Auth.GitHubLogin
{
    public sealed class GitHubLoginCommand : IRequest<LoginResponse>
    {
        public string Code { get; set; }
        public Role Role { get; set; }
    }
}
