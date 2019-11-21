using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jija.Services
{
    public class RevalidatingAuthStateProvider<TUser> : RevalidatingServerAuthenticationStateProvider
        where TUser : class
    {
        private readonly IdentityOptions _options;
        private readonly IServiceScopeFactory _scopeFactory;

        public RevalidatingAuthStateProvider(ILoggerFactory loggerFactory, IServiceScopeFactory scopeFactory,
            IOptions<IdentityOptions> options) : base(loggerFactory)
        {
            _scopeFactory = scopeFactory;
            _options = options.Value;
        }

        protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

        protected override async Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState,
            CancellationToken cancellationToken)
        {
            var scope = _scopeFactory.CreateScope();

            using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<TUser>>();
            return await ValidateSecurityStampAsync(userManager, authenticationState.User);
        }
        
        private async Task<bool> ValidateSecurityStampAsync(UserManager<TUser> userManager,
            ClaimsPrincipal principal)
        {
            var user = await userManager.GetUserAsync(principal);

            if (user is null) return false;

            if (!userManager.SupportsUserSecurityStamp) return true;

            var principalStamp = principal.FindFirstValue(_options.ClaimsIdentity.SecurityStampClaimType);
            var userStamp = await userManager.GetSecurityStampAsync(user);
            return principalStamp.Equals(user);
        }
    }
}