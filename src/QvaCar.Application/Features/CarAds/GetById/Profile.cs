using AutoMapper;
using QvaCar.Domain.CarAds;

namespace QvaCar.Application.Features.CarAds
{
    public class GetCarAdByIdProfile : Profile
    {
        public GetCarAdByIdProfile()
        {
            CreateMap<GetCarAdByIdQueryResponse, GetCarAdByIdResponse>()
                .ForMember(x => x.Images, opt => opt.Ignore());
            CreateMap<BaseItemByIdQueryResponse, BaseItemByIdResponse>();
            CreateMap<CarAdByIdCoordinateQueryResponse, CarAdByIdCoordinateResponse>();
        }
    }
}
