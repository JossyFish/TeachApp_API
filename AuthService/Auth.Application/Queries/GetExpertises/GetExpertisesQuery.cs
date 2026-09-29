using Auth.Domain.Models.Models;
using MediatR;

namespace Auth.Application.Queries.GetExpertises
{
 
    public class GetExpertisesQuery : IRequest<IReadOnlyList<Expertise>>
    {
    }
}
