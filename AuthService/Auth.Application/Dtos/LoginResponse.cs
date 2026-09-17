using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Application.Dtos
{
    public record LoginResponse(
        string AccessToken,
        string RefreshToken,
        Guid UserId,
        string Email,
        string Name,
        string LastName,
        IReadOnlyList<string?> Roles
    );
}
