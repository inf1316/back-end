using System.Threading.Tasks;

namespace QvaCar.Application.Services
{
    public interface IQvaCarAuthorizationService
    {
        public Task<bool> IsInRoleAsync(string role);
        public Task<bool> AuthorizeAsync(string policyName);
    }
}
