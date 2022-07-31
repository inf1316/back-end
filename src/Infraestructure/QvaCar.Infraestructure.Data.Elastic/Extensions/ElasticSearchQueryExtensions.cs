using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace QvaCar.Infraestructure.Data.Elastic.Queries
{
    static class ElasticSearchQueryExtensions
    {
        public static void AddTermsFilterUsingOrOperatorIfAnyValue<TModel, TValue>(
                            this List<QueryContainerDescriptor<TModel>> filters,
                                 TValue[] filterValues,
                                 Expression<Func<TModel, TValue>> filterFieldPath) where TModel : class
        {
            if (filterValues is null || !filterValues.Any())
                return;

            var query = new QueryContainerDescriptor<TModel>();
            query.Terms(t => t
                        .Field(filterFieldPath)
                        .Terms(filterValues)
                  );
            filters.Add(query);
        }

        public static void AddTermsFilterUsingAndOperatorIfAnyValue<TModel, TValue>(
                           this List<QueryContainerDescriptor<TModel>> filters,
                                TValue[] filterValues,
                                Expression<Func<TModel, IEnumerable<TValue>>> filterFieldPath) where TModel : class
        {
            if (filterValues is null || !filterValues.Any())
                return;

            var query = new QueryContainerDescriptor<TModel>();

            var boolQuery = new BoolQueryDescriptor<TModel>();
            foreach (var item in filterValues)
            {
                boolQuery.Should(s =>
                    s.Term(termquery => termquery
                        .Field(filterFieldPath)
                        .Value(item)
                    )
                );
            }
            query.Bool(_ => boolQuery);

            filters.Add(query);
        }

        public static void AddRangeFilterIncludingLimitsIfAnyValue<TModel>(
                            this List<QueryContainerDescriptor<TModel>> filterContextQuery,
                                 long? from, long? to,
                                 Expression<Func<TModel, long>> objectPath) where TModel : class
        {
            if (from is null && to is null)
                return;

            var range = new NumericRangeQueryDescriptor<TModel>();
            range.Field(objectPath);
            if (from is not null)
                range.GreaterThanOrEquals(from);

            if (to is not null)
                range.LessThanOrEquals(to);

            var queryContainer = new QueryContainerDescriptor<TModel>();
            queryContainer.Range(_ => range);
            filterContextQuery.Add(queryContainer);
        }

        public static void AddTermsAggregation<TModel, TValue>
                            (
                         this AggregationContainerDescriptor<TModel> aggregations,
                               string aggregationName,
                               Expression<Func<TModel, TValue>> filterFieldPath
                            ) where TModel : class
        {
            aggregations
                .Terms
                (aggregationName, t => t
                    .Field(filterFieldPath)
                    .Size(50)
                    .MinimumDocumentCount(0)
                    .Order(f => f.KeyAscending())
               );
        }
    }
}
