using Auth.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Auth.Application.Extensions
{
    public sealed class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(Permission permission)
                : base(policy: permission.ToString())
        {
        }
    }
}
