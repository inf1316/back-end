using MediatR;

namespace QvaCar.Application.Features.Identity
{
    public record ViewLoginQuery : IRequest<ViewLoginQueryResponse> { }
}
