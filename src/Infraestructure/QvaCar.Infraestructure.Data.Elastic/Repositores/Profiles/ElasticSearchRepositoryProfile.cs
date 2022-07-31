using AutoMapper;
using QvaCar.Domain.Search;
using QvaCar.Infraestructure.Data.Elastic.Entities;
using System.Linq;
using Geolocation = Nest.GeoLocation;

namespace QvaCar.Infraestructure.Data.Elastic.Repositores
{
    public class ElasticSearchRepositoryProfile : Profile
    {
        public ElasticSearchRepositoryProfile()
        {
            CreateMap<CarAdSearchPersistenceModel, CarAdSearchModel>()
                .ForMember(x => x.DomainEvents, opt => opt.Ignore());
            CreateMap<CarAdSearchModel, CarAdSearchPersistenceModel>()
                .ForMember(m=>m.ExteriorTypesIds,opt=>opt.MapFrom(source=>source.ExteriorTypes.Select(t=>t.Id).ToList()))
                .ForMember(m => m.InsideTypesIds, opt => opt.MapFrom(source => source.InsideTypes.Select(t => t.Id).ToList()))
                .ForMember(m => m.SafetyTypesIds, opt => opt.MapFrom(source => source.SafetyTypes.Select(t => t.Id).ToList()));
            CreateMap<Coordinate, Geolocation>().ConstructUsing(x => new Geolocation(x.Latitude, x.Longitude));
            CreateMap<Geolocation, Coordinate>()
                .ForMember(coordinate => coordinate.Latitude, opt => opt.MapFrom(geolocation => geolocation.Latitude))
                .ForMember(coordinate => coordinate.Longitude, opt => opt.MapFrom(geolocation => geolocation.Longitude));
        }
    }
}
