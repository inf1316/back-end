using MediatR;
using QvaCar.Domain.CarAds;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Application.Features.Identity
{
    public class ViewLoginQueryHandler : IRequestHandler<ViewLoginQuery, ViewLoginQueryResponse>
    {
        private readonly IProvinceRepository provinceRepository;

        public ViewLoginQueryHandler(IProvinceRepository _provinceRepository)
        {
            provinceRepository = _provinceRepository;
        }

        public Task<ViewLoginQueryResponse> Handle(ViewLoginQuery command, CancellationToken cancellationToken)
        {
            var provinces = provinceRepository.GetAll();
            var response = new ViewLoginQueryResponse()
            {
                Provinces = provinces.Select(data => new BaseReferenceDataItem() { Id = data.Id, Name = data.Name }).ToList(),
            };
            return Task.FromResult(response);
        }
    }
}