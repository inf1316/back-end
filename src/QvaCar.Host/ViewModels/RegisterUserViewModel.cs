using Microsoft.AspNetCore.Mvc.Rendering;

namespace QvaCar.Web.ViewModels
{
    public record RegisterUserViewModel
    {
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public string GivenName { get; init; } = string.Empty;
        public string FamilyName { get; init; } = string.Empty;
        public string Address { get; init; } = string.Empty;
        public int ProvinceId { get; init; }
        public int Age { get; init; }
        public SelectList? Provinces { get; init; }
        public string? ReturnUrl { get; init; }
    }
}
