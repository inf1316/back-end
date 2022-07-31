namespace QvaCar.Infraestructure.Data.Elastic.Configuration.Options
{
    public record ElasticSearchConfiguration
    {
        public readonly static string SectionName = "ElasticSearch";
        public string ServerEndpoint { get; set; } = string.Empty;
        public string CarAdsIndexName { get; set; } = string.Empty;
    }
}
