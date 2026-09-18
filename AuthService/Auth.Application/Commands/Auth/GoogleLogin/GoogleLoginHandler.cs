using Auth.Application.Dtos;
using Auth.Application.Interfaces;
using Auth.Domain.Enums;
using Auth.Domain.Interfaces;
using Auth.Domain.Models.Options;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.SqlServer.Server;

namespace Auth.Application.Commands.Auth.GoogleLogin
{
    public sealed class GoogleLoginHandler : IRequestHandler<GoogleLoginCommand, LoginResponse>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly ISessionCacheRepository _sessionCacheRepository;
        private readonly GoogleAuthOptions _googleOptions;
        private readonly JwtOptions _jwtOptions;

        public GoogleLoginHandler(IUsersRepository usersRepository, IJwtProvider jwtProvider, ISessionCacheRepository sessionCacheRepository, IOptions<GoogleAuthOptions> googleOptions,
                     IOptions<JwtOptions> jwtOptions)
        {
            _usersRepository = usersRepository;
            _jwtProvider = jwtProvider;
            _sessionCacheRepository = sessionCacheRepository;
            _googleOptions = googleOptions.Value;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<LoginResponse> Handle(GoogleLoginCommand command, CancellationToken ct)
        {
            GoogleJsonWebSignature.Payload payload;
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _googleOptions.ClientId }
                };
                payload = await GoogleJsonWebSignature.ValidateAsync(command.IdToken, settings);
            }
            catch (InvalidJwtException)
            {
                throw new InvalidGoogleTokenException();
            }

            if (!payload.EmailVerified)
                throw new EmailNotVerifiedException(payload.Email);

            var user = await _usersRepository.GetByEmailAsync(payload.Email, ct);

            if (user == null)
            {
                if (command.Role != Role.Student)
                    throw new RoleNotAllowedException(command.Role);


            }
        }

    }
}
