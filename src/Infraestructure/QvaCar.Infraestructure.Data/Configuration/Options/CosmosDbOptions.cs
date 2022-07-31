namespace QvaCar.Infraestructure.Configuration
{
    public class CosmosDbOptions
    {
        public const string Section = "CosmosDB";
        public string AccountEndpoint { get; init; } = string.Empty;
        public string AccountKey { get; init; } = string.Empty;
        public string DatabaseName { get; init; } = string.Empty;
    }
}