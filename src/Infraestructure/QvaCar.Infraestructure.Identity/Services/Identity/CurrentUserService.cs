using Microsoft.AspNetCore.Http;
using QvaCar.Application.Services;
using QvaCar.Infraestructure.Identity.Configuration;
using System;
using System.Security.Claims;

namespace QvaCar.Infraestructure.Identity.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public CurrentUser GetCurrentUser()
        {
            var user = _httpContextAccessor?.HttpContext?.User;
            if (user is null || !IsAuthenticated)
                throw new UnauthorizedAccessException("There is no user logged in");

            var userId = user.FindFirstValue(IdentityModel.JwtClaimTypes.Subject);
            var userFirstName = user.FindFirstValue(IdentityModel.JwtClaimTypes.GivenName);
            var userLastName = user.FindFirstValue(IdentityModel.JwtClaimTypes.FamilyName);
            var userProvince = user.FindFirstValue(QvaCarClaims.Province);
            var subscriptionLevel = user.FindFirstValue(QvaCarClaims.SubscriptionLevel);
            var response = new CurrentUser()
            {
                Id = Guid.Parse(userId),
                Name = $"{userFirstName} {userLastName}",
                Province = userProvince ?? "",
                SubscriptionLevel = subscriptionLevel,
            };
            return response;
        }
    }
}
