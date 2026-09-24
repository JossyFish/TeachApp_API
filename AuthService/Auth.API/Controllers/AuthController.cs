using Auth.Application.Commands.Auth.GitHubLogin;
using Auth.Application.Commands.Auth.GoogleLogin;
using Auth.Application.Commands.Auth.LoginStudent;
using Auth.Application.Commands.Delete.DeleteUser;
using Auth.Application.Commands.Register.ConfirmStudentRegisterCode;
using Auth.Application.Commands.Register.CreateStudent;
using Auth.Application.Extensions;
using Auth.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login-google")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginGoogle([FromBody] GoogleLoginCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Accepted(result);
        }

        [HttpPost("login-github")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginGitHub([FromBody] GitHubLoginCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Accepted(result);
        }

        [HttpPost("register-student")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterStudent([FromBody] CreateStudentCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return Accepted();
        }

        [HttpPost("confirm-registration-code-student")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmRegistrationByCodeStudent([FromBody] ConfirmStudentRegisterCodeCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Accepted(result);
        }

        [HttpPost("login-student")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginStudent([FromBody] LoginStudentCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Accepted(result);
        }

        [HttpDelete("delete-user")]
        //[HasPermission(Permission.ProfileEdit)]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

    }
}
