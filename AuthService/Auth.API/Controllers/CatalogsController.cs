using Auth.Application.Queries.GetExpertises;
using Auth.Application.Queries.GetSubscriptionPlans;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CatalogsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("expertises")]
        public async Task<IActionResult> GetExpertises(CancellationToken ct)
        {
            var result = await _mediator.Send(new GetExpertisesQuery(), ct);
            return Ok(result);
        }

        [HttpGet("subscription-plans")]
        public async Task<IActionResult> GetSubscriptionPlans(CancellationToken ct)
        {
            var result = await _mediator.Send(new GetSubscriptionPlansQuery(), ct);
            return Ok(result);
        }

    }
}
