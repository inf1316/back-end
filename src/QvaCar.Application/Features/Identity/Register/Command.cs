using MediatR;

namespace QvaCar.Application.Features.Identity
{
    public record RegisterUserCommand : IRequest
    {
        public string Email { get; init; } = string.Empty;
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public int Age { get; init; }
        public string Address { get; init; } = string.Empty;
        public int ProvinceId { get; init; }
        public string Password { get; set; } = string.Empty;

        public string EmailConfirmationUrlTemplate = string.Empty;
        public string EmailConfirmationUrlUserIdParameterName = string.Empty;
        public string EmailConfirmationUrlTokenParameterName = string.Empty;
    }
}
