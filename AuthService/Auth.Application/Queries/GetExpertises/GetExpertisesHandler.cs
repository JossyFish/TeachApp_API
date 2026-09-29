using Auth.Domain.Interfaces;
using Auth.Domain.Models.Models;
using MediatR;

namespace Auth.Application.Queries.GetExpertises
{
    public sealed class GetExpertisesHandler : IRequestHandler<GetExpertisesQuery, IReadOnlyList<Expertise>>
    {
        private readonly IUsersRepository _usersRepository;

        public GetExpertisesHandler(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<IReadOnlyList<Expertise>> Handle(GetExpertisesQuery request, CancellationToken cancellationToken)
        {
            return await _usersRepository.GetExpertisesAsync(cancellationToken);

        }

    }
}
