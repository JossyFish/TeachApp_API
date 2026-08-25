using Auth.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Auth.Application.Services
{
    public class PermissionRequirement(Permission[] permissions) : IAuthorizationRequirement
    {
        public Permission[] Permissions { get; set; } = permissions;
    }
}
