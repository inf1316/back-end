using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace QvaCar.Web.Common
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public abstract class UIControllerBase : Controller
    {
        private ISender? _mediator;
        private IMapper? _mapper;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>() ?? throw new System.Exception("Mediator is Required");
        protected IMapper Mapper => _mapper ??= HttpContext.RequestServices.GetService<IMapper>() ?? throw new System.Exception("Mapper is Required");
    }
}