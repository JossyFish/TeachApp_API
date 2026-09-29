using Auth.Domain.Models.Models;
using MediatR;

namespace Auth.Application.Queries.GetSubscriptionPlans
{
    public class GetSubscriptionPlansQuery : IRequest<IReadOnlyList<SubscriptionPlan>>
    {
    }
}
