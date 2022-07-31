using Microsoft.AspNetCore.Authorization;

namespace QvaCar.Api
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public abstract class AuthorizeApiControllerBase : ApiControllerBase { }
}