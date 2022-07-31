using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using QvaCar.Application.Services;
using System.Threading.Tasks;

namespace QvaCar.Infraestructure.Identity.Services
{
    public class QvaCarAuthorizationService: IQvaCarAuthorizationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthorizationService _authorizationService;
        public QvaCarAuthorizationService(IHttpContextAccessor httpContextAccessor, IAuthorizationService authorizationService)
        {
            _httpContextAccessor = httpContextAccessor;
            _authorizationService = authorizationService;
        }

        public Task<bool> IsInRoleAsync(string role)
        {
            var result = _httpContextAccessor?.HttpContext?.User?.IsInRole(role) ?? false;
            return Task.FromResult(result);
        }

        public async Task<bool> AuthorizeAsync(string policyName)
        {
            var user = _httpContextAccessor?.HttpContext?.User;
            if (user is null)
                return false;

            bool isAuthenticated = user.Identity?.IsAuthenticated == true;
            if (!isAuthenticated)
                return false;

            var result = await _authorizationService.AuthorizeAsync(user, policyName);

            return result.Succeeded;
        }
    }
}
