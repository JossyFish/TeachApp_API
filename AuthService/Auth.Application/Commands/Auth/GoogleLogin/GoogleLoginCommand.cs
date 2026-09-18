using Auth.Application.Dtos;
using Auth.Domain.Enums;
using MediatR;

namespace Auth.Application.Commands.Auth.GoogleLogin
{
    public record class GoogleLoginCommand : IRequest<LoginResponse>
    {
        public string IdToken { get; set; }
        public Role Role { get; set; } 
    }
}
