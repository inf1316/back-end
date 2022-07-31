using AutoMapper;
using QvaCar.Api.Features.CarAds.Requests;
using QvaCar.Application.Features.CarAds;
using System.IO;
using System.Linq;
using ApplicationNamespace = QvaCar.Application.Features.CarAds;

namespace QvaCar.Api.Features.CarAds
{
    public class CarAdControllerProfile : Profile
    {
        public CarAdControllerProfile()
        {
            CreateMap<RegisterCarAdRequest, ApplicationNamespace.RegisterCarAdCommand>();
            CreateMap<RegisterCarAdCoordinate, ApplicationNamespace.RegisterCarAdCoordinate>();
            CreateMap<UpdateCarAdRequest, ApplicationNamespace.UpdateCarAdCommand>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<UpdateCarAdCoordinate, ApplicationNamespace.UpdateCarAdCoordinate>();

            CreateMap<ApplicationNamespace.GetCarAdsByUserResponse, CarAdsResponse>()
                .ForMember(dest => dest.Ads, act => act.MapFrom(src => src.Ads));
            CreateMap<GetCarAdsImageByUserResponse, CarAdListImageResponse>();
            CreateMap<ImageByUserResponse, CarAdImage>();
            CreateMap<ApplicationNamespace.BaseItemByUserResponse, BaseItemListResponse>();

            CreateMap<ApplicationNamespace.CarAdByUserItemResponse, CarAdListItemResponse>();
            CreateMap<ApplicationNamespace.CarAdByUserListCoordinateResponse, CarAdByUserCoordinateResponse>();
            CreateMap<ApplicationNamespace.GetCarAdsReferenceDataResponse, CarAdReferenceDataResponse>();
            CreateMap<ApplicationNamespace.BaseReferenceDataItem, BaseReferenceDataItemResponse>();

            CreateMap<ApplicationNamespace.GetCarAdByIdResponse, CarAdByIdResponse>();
            CreateMap<ApplicationNamespace.GetCarAdImagebByIdResponse, CarAdImagebByIdResponse>();
            CreateMap<ApplicationNamespace.ImageByIdResponse, CarAdImageResponse>();
            CreateMap<ApplicationNamespace.CarAdByIdCoordinateResponse, CarAdByIdCoordinateResponse>();
            CreateMap<ApplicationNamespace.BaseItemByIdResponse, BaseItemByIdResponse>();

            CreateMap<AddImagesToCarAdRequest, ApplicationNamespace.AddImagesToCarAdCommand>()
                .ConvertUsing<HttpPostIFormFileTypeConverter>();
        }
    }

    internal class HttpPostIFormFileTypeConverter : ITypeConverter<AddImagesToCarAdRequest, AddImagesToCarAdCommand>
    {
        public AddImagesToCarAdCommand Convert(AddImagesToCarAdRequest source, AddImagesToCarAdCommand destination, ResolutionContext context)
        {
            return new AddImagesToCarAdCommand()
            {
                Images = source.Images.Select(r => new ImageStream()
                {
                    FileName = r.FileName,
                    File = ConvertStreamForByte.StreamToByteArray(r.OpenReadStream()),
                    ContentType = r.ContentType
                }).ToArray()
            };
        }
    }

    #region Helper
    internal class ConvertStreamForByte
    {
        internal static byte[] StreamToByteArray(Stream input)
        {
            using (var ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
    #endregion
}
