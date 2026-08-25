using Auth.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Auth.Application.Services
{
    public class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public PermissionAuthorizationPolicyProvider(IOptions<Microsoft.AspNetCore.Authorization.AuthorizationOptions> options)
                : base(options)
        {
        }

        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            AuthorizationPolicy? policy = await base.GetPolicyAsync(policyName);
            if (policy is not null)
            {
                return policy;
            }
            var permission = Enum.Parse<Permission>(policyName);
            return new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionRequirement([permission]))
                .Build();
        }
    }
}
