using Auth.Domain.Enums;

namespace Auth.Application.Interfaces
{
    public interface IPermissionService
    {
        Task<HashSet<Permission>> GetPermissionsAsync(Role role);
    }
}