using Auth.Application.Interfaces;
using Auth.Domain.Enums;
using Auth.Domain.Models.Options;
using Microsoft.Extensions.Options;

namespace Auth.Application.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly AuthorizationOptions _authorizationOptions;

        public PermissionService(IOptions<AuthorizationOptions> authorizationOptions)
        {
            _authorizationOptions = authorizationOptions.Value;
        }

        public Task<HashSet<Permission>> GetPermissionsAsync(Role role)
        {
            var rolePermissions = _authorizationOptions.RolePermissions;

            if (rolePermissions == null)
            {
                return Task.FromResult(new HashSet<Permission> { Permission.ProfileView });
            }

            var permissionStrings = rolePermissions
                .FirstOrDefault(rp => rp.Role == role.ToString())?
                .Permissions;

            if (permissionStrings == null)
                return Task.FromResult(new HashSet<Permission> { Permission.ProfileView });

            var permissions = permissionStrings
                .Select(p => Enum.Parse<Permission>(p))
                .ToHashSet();

            return Task.FromResult(permissions);
        }
    }
}
