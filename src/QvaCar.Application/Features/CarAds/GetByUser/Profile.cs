using AutoMapper;
using QvaCar.Domain.CarAds;

namespace QvaCar.Application.Features.CarAds
{
    public class GetCarAdsByUserProfile : Profile
    {
        public GetCarAdsByUserProfile()
        {
            CreateMap<GetCarAdListItemQueryResponse, CarAdByUserItemResponse>()
                .ForMember(x => x.Images, opt => opt.Ignore());
            CreateMap<BaseItemListQueryResponse, BaseItemByUserResponse>();
            CreateMap<CarAdByUserCoordinateQueryResponse, CarAdByUserListCoordinateResponse>();
        }
    }
}
