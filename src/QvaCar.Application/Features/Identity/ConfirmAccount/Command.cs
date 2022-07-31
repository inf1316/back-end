using MediatR;
using System;

namespace QvaCar.Application.Features.Identity
{
    public record ConfirmAccountCommand : IRequest<ConfirmAccountCommandResponse> 
    {
        public Guid UserId { get; init; }
        public string ConfirmAccountToken { get; set; } = string.Empty;
    }
}
