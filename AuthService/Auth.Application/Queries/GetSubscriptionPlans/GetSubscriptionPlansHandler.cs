using Auth.Domain.Interfaces;
using Auth.Domain.Models.Models;
using MediatR;

namespace Auth.Application.Queries.GetSubscriptionPlans
{
    public sealed class GetSubscriptionPlansHandler : IRequestHandler<GetSubscriptionPlansQuery, IReadOnlyList<SubscriptionPlan>>
    {
        private readonly IUsersRepository _usersRepository;

        public GetSubscriptionPlansHandler(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<IReadOnlyList<SubscriptionPlan>> Handle(GetSubscriptionPlansQuery request, CancellationToken cancellationToken)
        {
            return await _usersRepository.GetSubscriptionPlansAsync(cancellationToken);

        }


    }
}
