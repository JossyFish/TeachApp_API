using Auth.Application.Commands.Register.ConfirmStudentRegisterCode;
using Auth.Application.Commands.Register.CreateStudent;
using MediatR;
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

        [HttpPost("register-student")]
        public async Task<IActionResult> RegisterStudent([FromBody] CreateStudentCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return Accepted();
        }


        [HttpPost("confirm-registration-code-student")]
        public async Task<IActionResult> ConfirmRegistrationByCodeStudent([FromBody] ConfirmStudentRegisterCodeCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return Accepted();
        }

    }
}
