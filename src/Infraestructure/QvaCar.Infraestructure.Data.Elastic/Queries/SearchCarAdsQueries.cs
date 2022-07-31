using Nest;
using QvaCar.Domain.Search;
using QvaCar.Infraestructure.Data.Elastic.Entities;
using QvaCar.Infraestructure.Data.Elastic.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Infraestructure.Data.Elastic.Queries
{
    internal class SearchCarAdsQueries : ISearchCarAdsQueries
    {
        private readonly IElasticClient _elasticClient;

        public SearchCarAdsQueries(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<SearchCarAdModelsQueryResponse> SearchAsync(SearchCarAdModelsQueryRequest request, CancellationToken cancellationToken)
        {
            int from = request.PageSize * (request.Page - 1);

            var filterContextContainerDescriptor = GetQueryFilterContextFilters(request.Filters);
            var queryontextContainerDescriptor = GetQueryMustContextFilters(request.Filters);
            var sendAggregations = GetQueryAggregations();
            var scriptFields = GetScriptFields(request.Filters);

            var searchResponse = await _elasticClient.SearchAsync<CarAdSearchPersistenceModel>(s => s
               .StoredFields("_source")
               .Size(request.PageSize)
               .From(from)
               .Aggregations(_ => sendAggregations)
               .ScriptFields(_ => scriptFields)
               .Query(q => q
                    .Bool(boolQuery => boolQuery
                        .Filter(filterContextContainerDescriptor.ToArray())
                        .Must(queryontextContainerDescriptor.ToArray())
                    )
               ), cancellationToken);

            var querySent = System.Text.Encoding.UTF8.GetString(searchResponse.ApiCall.RequestBodyInBytes);

            if (searchResponse is null || !searchResponse.IsValid)
                throw new SearchQueryFailException("Fail to connect to Elastic Search.");

            if (!searchResponse.Documents.Any())
            {
                return new SearchCarAdModelsQueryResponse()
                {
                    Filters = new SearchCarAdModelsQueryFiltersResponse(),
                    Paging = SearchCarAdsModelsQueryPaging.CreateFor(0, request.Page, request.PageSize),
                    Results = new List<SearchCarAdModelsQueryItemResponse>(),
                };
            }

            var provincesResults = ReadTermsAggregationFromResponse(searchResponse, CarAdsQueryConstants.ProvinceAggName);
            var bodyTypesResults = ReadTermsAggregationFromResponse(searchResponse, CarAdsQueryConstants.BodyTypeAggName);
            var colorsResults = ReadTermsAggregationFromResponse(searchResponse, CarAdsQueryConstants.ColorAggName);
            var fuelTypesResults = ReadTermsAggregationFromResponse(searchResponse, CarAdsQueryConstants.FuelTypeAggName);
            var gearboxTypesResults = ReadTermsAggregationFromResponse(searchResponse, CarAdsQueryConstants.GearboxTypeAggName);

            var exteriorTypesTypesResults = ReadTermsAggregationFromResponse(searchResponse, CarAdsQueryConstants.ExteriorTypesIdsAggName);
            var insideTypesResults = ReadTermsAggregationFromResponse(searchResponse, CarAdsQueryConstants.InsideTypesIdsAggName);
            var safetyTypesResults = ReadTermsAggregationFromResponse(searchResponse, CarAdsQueryConstants.SafetyTypesAggName);

            var filtersResponse = new SearchCarAdModelsQueryFiltersResponse()
            {
                ProvincesIds = provincesResults,
                BodyTypesIds = bodyTypesResults,
                ColorsIds = colorsResults,
                FuelTypesIds = fuelTypesResults,
                GearboxTypesIds = gearboxTypesResults,
                ExteriorTypesIds = exteriorTypesTypesResults,
                InsideTypesIds = insideTypesResults,
                SafetyTypesIds = safetyTypesResults,
            };

            var results = new List<SearchCarAdModelsQueryItemResponse>();
            foreach (var hit in searchResponse.Hits)
            {
                var distanceFromLocation = ReadDistanceFromContacLocationFromResponse(hit, request.Filters, CarAdsQueryConstants.DistanceFromLocationScriptFieldName);
                var responseModel = BuildResponseItem(hit.Source, distanceFromLocation);
                results.Add(responseModel);
            }

            var paginationModel = SearchCarAdsModelsQueryPaging.CreateFor(searchResponse.Total, request.Page, request.PageSize);

            return new SearchCarAdModelsQueryResponse()
            {
                Results = results,
                Paging = paginationModel,
                Filters = filtersResponse,
            };
        }

        public async Task<SearchCarAdByIdQueryResponse?> SearchCarAdByIdAsync(Guid adId, CancellationToken cancellationToken)
        {
            var searchResponse = await _elasticClient.GetAsync<CarAdSearchPersistenceModel>(adId.ToString());

            if (searchResponse is null || searchResponse.OriginalException != null)
                throw new SearchQueryFailException("Fail to connect to Elastic Search.");

            if (!searchResponse.Found)
                return null;

            return SearchByIdResponseFromPersistence(searchResponse.Source);
        }

        private static List<QueryContainerDescriptor<CarAdSearchPersistenceModel>> GetQueryFilterContextFilters(SearchCarAdModelsSearchFilters? requestFilters)
        {
            var filterContextQuery = new List<QueryContainerDescriptor<CarAdSearchPersistenceModel>>();

            if (requestFilters is null)
                return filterContextQuery;

            filterContextQuery.AddTermsFilterUsingOrOperatorIfAnyValue(requestFilters.ProvincesIds, x => x.ProvinceId);
            filterContextQuery.AddTermsFilterUsingOrOperatorIfAnyValue(requestFilters.BodyTypesIds, x => x.BodyTypeId);
            filterContextQuery.AddTermsFilterUsingOrOperatorIfAnyValue(requestFilters.ColorsIds, x => x.ColorId);
            filterContextQuery.AddTermsFilterUsingOrOperatorIfAnyValue(requestFilters.FuelTypesIds, x => x.FuelTypeId);
            filterContextQuery.AddTermsFilterUsingOrOperatorIfAnyValue(requestFilters.GearboxTypesIds, x => x.GearboxTypeId);
            filterContextQuery.AddTermsFilterUsingAndOperatorIfAnyValue(requestFilters.ExteriorTypesIds, x => x.ExteriorTypesIds);
            filterContextQuery.AddTermsFilterUsingAndOperatorIfAnyValue(requestFilters.InsideTypesIds, x => x.InsideTypesIds);
            filterContextQuery.AddTermsFilterUsingAndOperatorIfAnyValue(requestFilters.SafetyTypesIds, x => x.SafetyTypesIds);

            filterContextQuery.AddRangeFilterIncludingLimitsIfAnyValue(requestFilters.Price?.From, requestFilters.Price?.To, x => x.Price);
            filterContextQuery.AddRangeFilterIncludingLimitsIfAnyValue(requestFilters.ManufacturingYear?.From, requestFilters.ManufacturingYear?.To, x => x.ManufacturingYear);
            filterContextQuery.AddRangeFilterIncludingLimitsIfAnyValue(requestFilters.Kilometers?.From, requestFilters.Kilometers?.To, x => x.Kilometers);
            AddContactLocationGeoDistanceFilter(requestFilters, filterContextQuery);

            return filterContextQuery;
        }

        private static void AddContactLocationGeoDistanceFilter(SearchCarAdModelsSearchFilters requestFilters, List<QueryContainerDescriptor<CarAdSearchPersistenceModel>> filterContextQuery)
        {
            if (requestFilters.ContactLocation is null)
                return;

            var contactLocationQuery = new QueryContainerDescriptor<CarAdSearchPersistenceModel>();
            contactLocationQuery.GeoDistance(geo =>
                geo
                   .Field(x => x.ContactLocation)
                   .Location(requestFilters.ContactLocation.Latitude, requestFilters.ContactLocation.Longitude)
                   .Distance(requestFilters.ContactLocation.DistanceInKilometers, DistanceUnit.Kilometers)
            );

            filterContextQuery.Add(contactLocationQuery);
        }

        private static List<QueryContainerDescriptor<CarAdSearchPersistenceModel>> GetQueryMustContextFilters(SearchCarAdModelsSearchFilters? requestFilters)
        {
            var contextQuery = new List<QueryContainerDescriptor<CarAdSearchPersistenceModel>>();

            if (requestFilters is null)
                return contextQuery;

            if (!string.IsNullOrEmpty(requestFilters.FreeText))
            {
                var filter = new MultiMatchQueryDescriptor<CarAdSearchPersistenceModel>();
                filter
                    .Fields(f => f
                                .Field(p => p.Description)
                                .Field(p => p.ModelVersion)
                           )
                    .Query(requestFilters.FreeText)
                    .Fuzziness(Fuzziness.Auto);

                var query = new QueryContainerDescriptor<CarAdSearchPersistenceModel>();
                query.MultiMatch(_ => filter);
                contextQuery.Add(query);
            }
            return contextQuery;
        }

        private static AggregationContainerDescriptor<CarAdSearchPersistenceModel> GetQueryAggregations()
        {
            var aggregations = new AggregationContainerDescriptor<CarAdSearchPersistenceModel>();

            aggregations.AddTermsAggregation(CarAdsQueryConstants.ProvinceAggName, model => model.ProvinceId);
            aggregations.AddTermsAggregation(CarAdsQueryConstants.BodyTypeAggName, model => model.BodyTypeId);
            aggregations.AddTermsAggregation(CarAdsQueryConstants.ColorAggName, model => model.ColorId);
            aggregations.AddTermsAggregation(CarAdsQueryConstants.FuelTypeAggName, model => model.FuelTypeId);
            aggregations.AddTermsAggregation(CarAdsQueryConstants.GearboxTypeAggName, model => model.GearboxTypeId);
            aggregations.AddTermsAggregation(CarAdsQueryConstants.ExteriorTypesIdsAggName, model => model.ExteriorTypesIds);
            aggregations.AddTermsAggregation(CarAdsQueryConstants.InsideTypesIdsAggName, model => model.InsideTypesIds);
            aggregations.AddTermsAggregation(CarAdsQueryConstants.SafetyTypesAggName, model => model.SafetyTypesIds);

            return aggregations;
        }

        private static List<SearchCarAdsModelsFilterItemResultBase> ReadTermsAggregationFromResponse(ISearchResponse<CarAdSearchPersistenceModel> searchResponse, string aggregationName)
        {
            var aggregationRawItems = searchResponse.Aggregations.Terms(aggregationName);

            if (aggregationRawItems is null)
                return new List<SearchCarAdsModelsFilterItemResultBase>();

            var result = new List<SearchCarAdsModelsFilterItemResultBase>();
            foreach (var bucket in aggregationRawItems.Buckets)
            {
                result.Add(new SearchCarAdsModelsFilterItemResultBase(bucket.Key, bucket.DocCount ?? 0));
            }
            return result;
        }

        private static ScriptFieldsDescriptor GetScriptFields(SearchCarAdModelsSearchFilters? requestFilters)
        {
            if (requestFilters is null)
                return new ScriptFieldsDescriptor();

            var scriptFields = new ScriptFieldsDescriptor();

            var contactLocation = requestFilters.ContactLocation;
            if (contactLocation is not null)
            {
                scriptFields.ScriptField(CarAdsQueryConstants.DistanceFromLocationScriptFieldName,
                    sf => sf.Source($"doc['contactLocation'].arcDistance(params.lat,params.lon) * 0.001")
                            .Params(@params => @params.Add("lat", contactLocation.Latitude).Add("lon", contactLocation.Longitude))
                            .Lang("painless")
                );
            }

            return scriptFields;
        }
        private double? ReadDistanceFromContacLocationFromResponse(IHit<CarAdSearchPersistenceModel> responseItem, SearchCarAdModelsSearchFilters? requestFilters, string distanceFromLocationScriptFieldName)
        {
            if (requestFilters?.ContactLocation is null)
                return null;
            return responseItem.Fields.Value<double>(distanceFromLocationScriptFieldName);
        }

        private static SearchCarAdModelsQueryItemResponse BuildResponseItem(CarAdSearchPersistenceModel doc, double? distanceFromLocation)
        {
            return new SearchCarAdModelsQueryItemResponse()
            {
                Id = doc.Id.ToString(),
                UserId = doc.UserId.ToString(),
                CreatedAt = doc.CreatedAt,
                UpdatedAt = doc.UpdatedAt,
                StateId = doc.StateId,
                StateName = doc.StateName,
                Price = doc.Price,
                ProvinceId = doc.ProvinceId,
                ProvinceName = doc.ProvinceName,
                ManufacturingYear = doc.ManufacturingYear,
                Kilometers = doc.Kilometers,
                BodyTypeId = doc.BodyTypeId,
                BodyTypeName = doc.BodyTypeName,
                ColorId = doc.ColorId,
                ColorName = doc.ColorName,
                FuelTypeId = doc.FuelTypeId,
                FuelTypeName = doc.FuelTypeName,
                GearboxTypeId = doc.GearboxTypeId,
                GearboxTypeName = doc.GearboxTypeName,
                Description = doc.Description,
                ContactPhoneNumber = doc.ContactPhoneNumber,
                ModelVersion = doc.ModelVersion,
                ContactLocation = GetCoordinateFromPersistence(doc.ContactLocation, distanceFromLocation),
                ExteriorTypes = doc.ExteriorTypes.Select(x => new BaseSearchQueryEnumItemResponse(x.Id, x.Name)).ToArray(),
                InsideTypes = doc.InsideTypes.Select(x => new BaseSearchQueryEnumItemResponse(x.Id, x.Name)).ToArray(),
                SafetyTypes = doc.SafetyTypes.Select(x => new BaseSearchQueryEnumItemResponse(x.Id, x.Name)).ToArray(),
                Images = doc.Images.ToArray(),
            };
        }

        private static SearchCarAdsModelsCoordinateListItemResponse? GetCoordinateFromPersistence(GeoLocation? location, double? distanceFromLocation)
        {
            if (location is null)
                return null;

            return new SearchCarAdsModelsCoordinateListItemResponse()
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                DistanceFromContactLocationFilterInKilometers = distanceFromLocation is not null ? Math.Round(distanceFromLocation.Value, 2) : null
            };
        }

        private SearchCarAdByIdQueryResponse? SearchByIdResponseFromPersistence(CarAdSearchPersistenceModel doc)
        {
            return new SearchCarAdByIdQueryResponse()
            {
                Id = doc.Id,
                UserId = doc.UserId,
                CreatedAt = doc.CreatedAt,
                UpdatedAt = doc.UpdatedAt,
                StateId = doc.StateId,
                StateName = doc.StateName,
                Price = doc.Price,
                ProvinceId = doc.ProvinceId,
                ProvinceName = doc.ProvinceName,
                ManufacturingYear = doc.ManufacturingYear,
                Kilometers = doc.Kilometers,
                BodyTypeId = doc.BodyTypeId,
                BodyTypeName = doc.BodyTypeName,
                ColorId = doc.ColorId,
                ColorName = doc.ColorName,
                FuelTypeId = doc.FuelTypeId,
                FuelTypeName = doc.FuelTypeName,
                GearboxTypeId = doc.GearboxTypeId,
                GearboxTypeName = doc.GearboxTypeName,
                Description = doc.Description,
                ContactPhoneNumber = doc.ContactPhoneNumber,
                ModelVersion = doc.ModelVersion,
                ContactLocation = GetCoordinateByIdResponseFromPersistence(doc.ContactLocation),
                ExteriorTypes = doc.ExteriorTypes.Select(x => new SearchBaseItemByIdQueryResponse(x.Id, x.Name)).ToArray(),
                InsideTypes = doc.InsideTypes.Select(x => new SearchBaseItemByIdQueryResponse(x.Id, x.Name)).ToArray(),
                SafetyTypes = doc.SafetyTypes.Select(x => new SearchBaseItemByIdQueryResponse(x.Id, x.Name)).ToArray(),
                Images = doc.Images.ToArray(),
            };
        }

        private static SearchCarAdByIdCoordinateQueryResponse? GetCoordinateByIdResponseFromPersistence(GeoLocation? location)
        {
            if (location is null)
                return null;

            return new SearchCarAdByIdCoordinateQueryResponse()
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude,
            };
        }
    }
}